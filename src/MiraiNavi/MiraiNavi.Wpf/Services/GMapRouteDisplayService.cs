using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    int _positionIndex;
    readonly List<GMapMarker> _routeMarkers = [];
    readonly List<UtcTime> _timeStamps = [];
    readonly static Brush _beforeFill = (Brush)App.Current.Resources["MikuRedBrush"];
    readonly static Brush _afterFill = (Brush)App.Current.Resources["MikuGreenBrush"];
    CancellationTokenSource? _cancellationTokenSource;

    static GMapMarker CreateEllipseMarker(Brush fill, PointLatLng point)
    {
        var shape = new Ellipse
        {
            Fill = fill,
            Width = 5,
            Height = 5,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        return new GMapMarker(point)
        {
            Shape = shape,
            Offset = new(-shape.Width / 2, -shape.Height / 2)
        };
    }

    void ThrowIfIsRunnning()
    {
        if (_cancellationTokenSource != null && !_cancellationTokenSource.IsCancellationRequested)
            throw new InvalidOperationException("The display is running.");
    }

    public void AddPoint(PointLatLng point, UtcTime timeStamp)
    {
        ThrowIfIsRunnning();
        ArgumentNullException.ThrowIfNull(_gMapControl);
        ArgumentNullException.ThrowIfNull(_positionMarker);
        if (_routeMarkers.Count == 0)
            _positionMarker.Position = point;
        var marker = CreateEllipseMarker(_afterFill, point);
        _routeMarkers.Add(marker);
        _timeStamps.Add(timeStamp);
        _gMapControl.Markers.Add(marker);
        _gMapControl.Markers.Remove(_positionMarker);
        _gMapControl.Markers.Add(_positionMarker);
    }

    public void ClearPoints()
    {
        ThrowIfIsRunnning();
        ArgumentNullException.ThrowIfNull(_gMapControl);
        ArgumentNullException.ThrowIfNull(_positionMarker);
        foreach (var marker in _routeMarkers)
            _gMapControl.Markers.Remove(marker);
        _positionMarker.Shape.Visibility = Visibility.Collapsed;
        _routeMarkers.Clear();
        _timeStamps.Clear();
        _positionIndex = default;
    }

    public void Pause()
    {
        if (_cancellationTokenSource is null)
            return;
        if (!_cancellationTokenSource.IsCancellationRequested)
            _cancellationTokenSource.Cancel();
    }

    public IGMapRouteDisplayService RegisterGMapControl(GMapControl gMapControl)
    {
        _gMapControl = gMapControl;
        return this;
    }

    public IGMapRouteDisplayService RegisterPositionMarker(GMapMarker positionMarker)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        _positionMarker = positionMarker;
        _positionMarker.Shape.Visibility = Visibility.Collapsed;
        _gMapControl.Markers.Add(_positionMarker);
        return this;
    }

    static void SetEllipseMarkerShape(GMapMarker marker, Brush fill)
        => ((Ellipse)marker.Shape).Fill = fill;


    public void MoveTo(int newIndex)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(newIndex, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(newIndex, _routeMarkers.Count - 1);
        if (_positionIndex == newIndex)
            return;
        ArgumentNullException.ThrowIfNull(_positionMarker);
        ThrowIfIsRunnning();
        _positionMarker.Shape.Visibility = Visibility.Visible;
        _positionMarker.Position = _routeMarkers[newIndex].Position;
        if (_positionIndex > newIndex)
        {
            for (int i = newIndex + 1; i <= _positionIndex; i++)
                SetEllipseMarkerShape(_routeMarkers[i], _afterFill);
        }
        else
        {
            for (int i = _positionIndex; i < newIndex; i++)
                SetEllipseMarkerShape(_routeMarkers[i], _beforeFill);
        }
        _positionIndex = newIndex;
    }

    public async Task StartAsync(double timeScale = 1)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        ArgumentNullException.ThrowIfNull(_positionMarker);
        if (_routeMarkers.Count == 0 || _cancellationTokenSource is not null)
            return;
        _cancellationTokenSource = new();
        _positionMarker.Shape.Visibility = Visibility.Visible;
        try
        {
            await Task.Run(async () =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested && _positionIndex < _routeMarkers.Count - 1)
                {
                    var duration = (_timeStamps![_positionIndex + 1] - _timeStamps[_positionIndex]) * timeScale;
                    await Task.Delay(duration, _cancellationTokenSource.Token);
                    //Thread.Sleep();
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        _positionMarker.Position = _routeMarkers[_positionIndex + 1].Position;
                        SetEllipseMarkerShape(_routeMarkers[_positionIndex], _beforeFill);
                    });
                    _positionIndex++;
                }
            });
        }
        catch (TaskCanceledException)
        {

        }
        finally
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = default;
        }
    }

    public void Stop()
    {
        Pause();
        MoveTo(0);
    }

    public void MoveToOffset(int offset) => MoveTo(_positionIndex + offset);

}
