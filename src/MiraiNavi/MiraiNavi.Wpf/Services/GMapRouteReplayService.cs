using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using MiraiNavi.WpfApp.Services.Contracts;
using Wpf.Ui.Interop.WinDef;

namespace MiraiNavi.WpfApp.Services;

public class GMapRouteReplayService : IGMapRouteReplayService
{
    GMapControl? _gMapControl;
    Brush? _beforeFill;
    Brush? _afterFill;
    ImmutableList<UtcTime>? _timeStamps;
    GMapMarker? _positionMarker;
    ImmutableList<GMapMarker>? _routeMakers;
    readonly CancellationTokenSource _cancellationTokenSource = new();
    int _index = 0;

    public PointLatLng CurrentPosition
    {
        get
        {
            ThrowExceptionIfNotInitilized();
            return _positionMarker!.Position;
        }
    }

    public IGMapRouteReplayService Initiliaze(GMapControl gMapControl, GMapMarker positionMarker, Brush beforeFill, Brush afterFill)
    {
        _gMapControl = gMapControl;
        _positionMarker = positionMarker;
        _beforeFill = beforeFill;
        _afterFill = afterFill;
        _positionMarker.Shape.Visibility = Visibility.Collapsed;
        return this;
    }

    public void SetPosition(int index)
    {
        ThrowExceptionIfNotInitilized();
        ThrowExceptionIfRouteIsNotSet();
        _positionMarker!.Shape.Visibility = Visibility.Visible;
        Pause();
        _index = index;
        foreach (var marker in _routeMakers!.Take(index))
            ((Ellipse)marker.Shape).Fill = _beforeFill;
        foreach (var marker in _routeMakers!.Skip(index + 1))
            ((Ellipse)marker.Shape).Fill = _afterFill;
        _positionMarker.Position = _routeMakers![index].Position;
    }

    public void SetRoute(IEnumerable<PointLatLng> points, IEnumerable<UtcTime> timeStamps)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        if (!points.Any())
            throw new ArgumentException("The points is empty");
        if (points.Count() != timeStamps.Count())
            throw new ArgumentException("");
        if (_routeMakers is not null)
            foreach (var marker in _routeMakers)
                _gMapControl.Markers.Remove(marker);
        _routeMakers = points.Select(p =>
        {
            var shape = new Ellipse { Fill = _afterFill, Width = 5, Height = 5, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            var marker = new GMapMarker(p) { Shape = shape, Offset = new(-shape.Width / 2, -shape.Height / 2) };
            return marker;
        }).ToImmutableList();
        foreach (var marker in _routeMakers)
            _gMapControl.Markers.Add(marker);
        _gMapControl.Markers.Add(_positionMarker);
        _timeStamps = timeStamps.ToImmutableList();
        Reset();
        _gMapControl.CenterPosition = _positionMarker!.Position;
    }

    void ThrowExceptionIfRouteIsNotSet()
    {
        if (_routeMakers is null || _timeStamps is null)
            throw new InvalidOperationException("The route is not set");
    }

    void ThrowExceptionIfNotInitilized()
    {
        if (_positionMarker is null)
            throw new InvalidOperationException("The service is not initialized");
    }

    public async Task StartAsync(double timeScale = 1)
    {
        ThrowExceptionIfNotInitilized();
        ThrowExceptionIfRouteIsNotSet();
        _positionMarker!.Shape.Visibility = Visibility.Visible;
        await Task.Run(() =>
        {
            while (_index < _routeMakers!.Count - 1)
            {
                if (_cancellationTokenSource.IsCancellationRequested)
                    return;
                var timeSpan = (_timeStamps![_index + 1] - _timeStamps[_index]) * timeScale;
                Thread.Sleep(timeSpan);
                App.Current.Dispatcher.Invoke(() =>
                {
                    ((Ellipse)_routeMakers[_index].Shape).Fill = _beforeFill;
                    _positionMarker!.Position = _routeMakers[_index + 1].Position;
                });
                _index++;
            }
        });
    }

    void Reset()
    {
        ThrowExceptionIfNotInitilized();
        ThrowExceptionIfRouteIsNotSet();
        _routeMakers!.ForEach(m => ((Ellipse)m.Shape).Fill = _afterFill);
        _index = 0;
        _positionMarker!.Position = _routeMakers[_index].Position;
    }

    public void Stop()
    {
        Pause();
        Reset();
        _positionMarker!.Shape.Visibility = Visibility.Collapsed;
    }

    public void Pause()
    {
        if (_cancellationTokenSource.IsCancellationRequested)
            return;
        _cancellationTokenSource.Cancel();
    }
}
