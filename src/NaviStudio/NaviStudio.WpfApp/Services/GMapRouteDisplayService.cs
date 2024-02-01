using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using NaviStudio.WpfApp.Services.Contracts;
using System.Windows.Media;
using NaviStudio.WpfApp.Common.Extensions;

namespace NaviStudio.WpfApp.Services;

//TODO 地图标记点优化
public partial class GMapRouteDisplayService : IGMapRouteDisplayService
{

    public PointLatLng? CurrentPosition => _positionIndex == -1 ? null : _routeMarkers[_positionIndex].Position;

    public void AddPoint(PointLatLng point, UtcTime timeStamp, bool updatePositionMarker = false)
    {
        ThrowIfIsRunnning();
        ArgumentNullException.ThrowIfNull(_gMapControl);
        App.Current.Dispatcher.Invoke(() =>
        {
            _gMapControl.Markers.Remove(_positionMarker);
            var marker = CreateRouteMarker(_afterFill, point, timeStamp);
            _routeMarkers.Add(marker);
            _timeStamps.Add(timeStamp);
            _gMapControl.Markers.Add(marker);
            if(updatePositionMarker)
                UpdatePositionMarker(point);
            _positionMarker.Shape.Visibility = Visibility.Visible;
            _gMapControl.Markers.Add(_positionMarker);
        });
    }

    public void Clear()
    {
        ThrowIfIsRunnning();
        ArgumentNullException.ThrowIfNull(_gMapControl);
        foreach(var marker in _routeMarkers)
            _gMapControl.Markers.Remove(marker);
        _positionMarker.Shape.Visibility = Visibility.Collapsed;
        _routeMarkers.Clear();
        _timeStamps.Clear();
        _positionIndex = -1;
    }

    public IGMapRouteDisplayService RegisterGMapControl(GMapControl gMapControl)
    {
        if(_gMapControl is not null)
            throw new InvalidOperationException("The GMapControl has been registered.");
        _gMapControl = gMapControl;
        SetPositionMarkerShape();
        _gMapControl.Markers.Add(_positionMarker);
        _gMapControl.OnMapZoomChanged += Cluster;
        _gMapControl.OnPositionChanged += (_) => Cluster();
        _gMapControl.OnMapDrag += Cluster;
        return this;
    }

    public void MoveTo(int newIndex)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(newIndex, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(newIndex, _routeMarkers.Count - 1);
        if(_positionIndex == newIndex)
            return;
        ThrowIfIsRunnning();
        UpdatePositionMarker(_routeMarkers[newIndex].Position);
        if(_positionIndex > newIndex)
        {
            for(int i = newIndex + 1; i <= _positionIndex; i++)
                SetEllipseMarkerFill(_routeMarkers[i], _afterFill);
        }
        else
        {
            for(int i = _positionIndex; i < newIndex; i++)
                SetEllipseMarkerFill(_routeMarkers[i], _beforeFill);
        }
        _positionIndex = newIndex;
    }

    public async Task StartAsync(CancellationToken token, double timeScale = 1)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        if(_routeMarkers.Count == 0)
            throw new InvalidOperationException("No points.");
        ThrowIfIsRunnning();
        _positionMarker.Shape.Visibility = Visibility.Visible;
        _isRunning = true;
        try
        {
            await Task.Run(async () =>
            {
                while(!token.IsCancellationRequested && _positionIndex < _routeMarkers.Count - 1)
                {
                    var duration = (_timeStamps![_positionIndex + 1] - _timeStamps[_positionIndex]) * timeScale;
                    await Task.Delay(duration, token);
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        UpdatePositionMarker(_routeMarkers[_positionIndex + 1].Position);
                        SetEllipseMarkerFill(_routeMarkers[_positionIndex], _beforeFill);
                    });
                    _positionIndex++;
                }
            }, token);
        }
        catch(Exception)
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
        if(!points.Any())
            return;
        App.Current.Dispatcher.Invoke(() =>
        {
            _gMapControl.Markers.Remove(_positionMarker);
            foreach((var point, var timeStamp) in points)
            {
                var marker = CreateRouteMarker(_afterFill, point, timeStamp);
                _routeMarkers.Add(marker);
                _timeStamps.Add(timeStamp);
                _gMapControl.Markers.Add(marker);
            }
            if(updatePositionMarker)
                UpdatePositionMarker(_routeMarkers[^1].Position);
            _positionMarker.Shape.Visibility = Visibility.Visible;
            _gMapControl.Markers.Add(_positionMarker);
        });
    }

    readonly static Brush _beforeFill = (Brush)App.Current.Resources["MikuRedBrush"];

    readonly static Brush _afterFill = (Brush)App.Current.Resources["MikuGreenBrush"];

    readonly List<GMapMarker> _routeMarkers = [];

    readonly List<UtcTime> _timeStamps = [];

    GMapControl? _gMapControl;
    readonly GMapMarker _positionMarker = new(new());
    int _positionIndex = -1;
    bool _isRunning = false;
    // double _groundResolution;

    PureProjection Projection => _gMapControl!.MapProvider.Projection;

    static GMapMarker CreateRouteMarker(Brush fill, PointLatLng point, UtcTime timeStamp)
    {
        var shape = new Ellipse
        {
            Fill = fill,
            Width = 5,
            Height = 5,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Tag = new TimePointLatLng(timeStamp, point),
            ToolTip = $"{timeStamp:HH:mm:ss.fff}{Environment.NewLine}{point}"
        };
        return new GMapMarker(point).SetShape(shape);
    }

    static void SetEllipseMarkerFill(GMapMarker marker, Brush fill)
    {
        var e = (Ellipse)marker.Shape;
        e.Fill = fill;
    }

    void ThrowIfIsRunnning()
    {
        if(_isRunning)
            throw new InvalidOperationException("The display is running.");
    }

    //void UpdateGroundResolution()
    //{
    //    ArgumentNullException.ThrowIfNull(_gMapControl);
    //    _groundResolution = Projection.GetGroundResolution((int)_gMapControl.Zoom, _gMapControl.Position.Lat);
    //}
}
