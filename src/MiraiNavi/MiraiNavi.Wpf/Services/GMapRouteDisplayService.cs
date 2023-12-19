using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using MiraiNavi.WpfApp.Services.Contracts;
using System.Windows.Media;

namespace MiraiNavi.WpfApp.Services;

public class GMapRouteDisplayService : IGMapRouteDisplayService
{
    GMapControl? _gMapControl;
    GMapMarker? _positionMarker;
    int _positionIndex = -1;
    readonly List<GMapMarker> _routeMarkers = [];
    readonly List<UtcTime> _timeStamps = [];
    readonly HashSet<GMapMarker> _disabledMarkers = [];
    readonly static Brush _beforeFill = (Brush)App.Current.Resources["MikuRedBrush"];
    readonly static Brush _afterFill = (Brush)App.Current.Resources["MikuGreenBrush"];
    bool _isRunning = false;
    double _groundResolution;

    PureProjection Projection => _gMapControl!.MapProvider.Projection;

    public PointLatLng? CurrentPosition => _positionIndex == -1 ? null : _routeMarkers[_positionIndex].Position;

    static GMapMarker CreateEllipseMarker(Brush fill, PointLatLng point, UtcTime timeStamp)
    {
        var shape = new Ellipse
        {
            Fill = fill,
            Width = 5,
            Height = 5,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Tag = (point, timeStamp)
        };
        return new GMapMarker(point)
        {
            Shape = shape,
            Offset = new(-shape.Width / 2, -shape.Height / 2)
        };
    }

    void ThrowIfIsRunnning()
    {
        if (_isRunning)
            throw new InvalidOperationException("The display is running.");
    }

    public void AddPoint(PointLatLng point, UtcTime timeStamp, bool updatePositionMarker = false)
    {
        ThrowIfIsRunnning();
        ArgumentNullException.ThrowIfNull(_gMapControl);
        ArgumentNullException.ThrowIfNull(_positionMarker);
        App.Current.Dispatcher.Invoke(() =>
        {
            _gMapControl.Markers.Remove(_positionMarker);
            var marker = CreateEllipseMarker(_afterFill, point, timeStamp);
            _routeMarkers.Add(marker);
            _timeStamps.Add(timeStamp);
            _gMapControl.Markers.Add(marker);
            if (updatePositionMarker)
            {
                _positionMarker.Position = point;
                _positionIndex = _routeMarkers.Count - 1;
            }
            _positionMarker.Shape.Visibility = Visibility.Visible;
            _gMapControl.Markers.Add(_positionMarker);
        });
        TryOptimize();
    }

    public void Clear()
    {
        ThrowIfIsRunnning();
        ArgumentNullException.ThrowIfNull(_gMapControl);
        ArgumentNullException.ThrowIfNull(_positionMarker);
        foreach (var marker in _routeMarkers)
            _gMapControl.Markers.Remove(marker);
        _positionMarker.Shape.Visibility = Visibility.Collapsed;
        _routeMarkers.Clear();
        _timeStamps.Clear();
        _disabledMarkers.Clear();
        _positionIndex = -1;
    }

    void UpdateGroundResolution()
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        _groundResolution = Projection.GetGroundResolution((int)_gMapControl.Zoom, _gMapControl.Position.Lat);
    }

    public IGMapRouteDisplayService RegisterGMapControl(GMapControl gMapControl)
    {
        _gMapControl = gMapControl;
        _gMapControl.OnMapDrag += () => TryOptimize();
        _gMapControl.OnMapZoomChanged += () =>
        {
            UpdateGroundResolution();
            TryOptimize();
        };
        _gMapControl.OnPositionChanged += (p) => TryOptimize();
        _gMapControl.SizeChanged += (sender, e) => TryOptimize();
        UpdateGroundResolution();
        return this;
    }

    const int _maxVisibleMarkers = 1000;

    bool NeedOptimize => _routeMarkers.Count > _maxVisibleMarkers;

    void TryOptimize()
    {
        if (!NeedOptimize)
            return;
        var visibleMarkers = DisableMarkersOutsideOfViewArea();
        var clusterLevel = 1;
        while (clusterLevel <= 10 && visibleMarkers.Count() > _maxVisibleMarkers)
        {
            visibleMarkers = ClusterMarkers(visibleMarkers, clusterLevel * 3);
            clusterLevel++;
        }
    }

    void EnableMarker(GMapMarker marker)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        if (_disabledMarkers.Remove(marker))
            App.Current.Dispatcher.Invoke(() => _gMapControl.Markers.Add(marker));
    }

    void DisableMarker(GMapMarker marker)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        if (_disabledMarkers.Add(marker))
            App.Current.Dispatcher.Invoke(() => _gMapControl.Markers.Remove(marker));
    }

    IEnumerable<GMapMarker> DisableMarkersOutsideOfViewArea()
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        var viewArea = _gMapControl.ViewArea;
        for (int i = 0; i < _routeMarkers.Count; i++)
        {
            var marker = _routeMarkers[i];
            if (!viewArea.Contains(marker.Position))
            {
                DisableMarker(marker);
                continue;
            }
            EnableMarker(marker);
            yield return marker;
        }
    }

    IEnumerable<GMapMarker> ClusterMarkers(IEnumerable<GMapMarker> visibleMarkers, double maxDistance)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        GMapMarker? preMarker = default;
        var maxDistanceSquare = Math.Pow(maxDistance, 2);
        foreach (var marker in visibleMarkers)
        {
            if (preMarker == null)
            {
                preMarker = marker;
                continue;
            }
            var distance = Math.Pow(marker.LocalPositionX - preMarker.LocalPositionX, 2) + Math.Pow(marker.LocalPositionY - preMarker.LocalPositionY, 2);
            if (distance < maxDistanceSquare)
            {
                DisableMarker(marker);
                continue;
            }
            preMarker = marker;
            yield return marker;
        }
    }

    public IGMapRouteDisplayService RegisterPositionMarker(GMapMarker positionMarker)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        _positionMarker = positionMarker;
        _positionMarker.Shape.Visibility = Visibility.Collapsed;
        _gMapControl.Markers.Add(_positionMarker);
        return this;
    }

    static void SetEllipseMarkerFill(GMapMarker marker, Brush fill)
    {
        var e = (Ellipse)marker.Shape;
        e.Fill = fill;
    }

    public void MoveTo(int newIndex)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(newIndex, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(newIndex, _routeMarkers.Count - 1);
        if (_positionIndex == newIndex)
            return;
        ArgumentNullException.ThrowIfNull(_positionMarker);
        ThrowIfIsRunnning();
        _positionMarker.Position = _routeMarkers[newIndex].Position;
        if (_positionIndex > newIndex)
        {
            for (int i = newIndex + 1; i <= _positionIndex; i++)
                SetEllipseMarkerFill(_routeMarkers[i], _afterFill);
        }
        else
        {
            for (int i = _positionIndex; i < newIndex; i++)
                SetEllipseMarkerFill(_routeMarkers[i], _beforeFill);
        }
        _positionIndex = newIndex;
    }

    public async Task StartAsync(CancellationToken token, double timeScale = 1)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        ArgumentNullException.ThrowIfNull(_positionMarker);
        if (_routeMarkers.Count == 0)
            throw new InvalidOperationException("No points.");
        ThrowIfIsRunnning();
        _positionMarker.Shape.Visibility = Visibility.Visible;
        _isRunning = true;
        try
        {
            await Task.Run(async () =>
            {
                while (!token.IsCancellationRequested && _positionIndex < _routeMarkers.Count - 1)
                {
                    var duration = (_timeStamps![_positionIndex + 1] - _timeStamps[_positionIndex]) * timeScale;
                    await Task.Delay(duration, token);
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        _positionMarker.Position = _routeMarkers[_positionIndex + 1].Position;
                        SetEllipseMarkerFill(_routeMarkers[_positionIndex], _beforeFill);
                    });
                    _positionIndex++;
                }
            }, token);
        }
        catch (Exception)
        {

        }
        finally
        {
            _isRunning = false;
        }
    }

    public void MoveToOffset(int offset) => MoveTo(_positionIndex + offset);

    public void AddPoints(IEnumerable<(PointLatLng, UtcTime)> points, bool updatePositionMarker = false)
    {
        ThrowIfIsRunnning();
        ArgumentNullException.ThrowIfNull(_gMapControl);
        ArgumentNullException.ThrowIfNull(_positionMarker);
        if (!points.Any())
            return;
        _gMapControl.Markers.Remove(_positionMarker);
        foreach ((var point, var timeStamp) in points)
        {
            var marker = CreateEllipseMarker(_afterFill, point, timeStamp);
            _routeMarkers.Add(marker);
            _timeStamps.Add(timeStamp);
            _gMapControl.Markers.Add(marker);
        }
        if (updatePositionMarker)
        {
            _positionMarker.Position = _routeMarkers[^1].Position;
            _positionIndex = _routeMarkers.Count - 1;
        }
        _positionMarker.Shape.Visibility = Visibility.Visible;
        _gMapControl.Markers.Add(_positionMarker);
        TryOptimize();
    }
}
