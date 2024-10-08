﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using NaviStudio.WpfApp.Services.Contracts;
using System.Windows.Media;
using NaviStudio.WpfApp.Common.Extensions;
using NaviStudio.Shared.Models.Map;
using System.Diagnostics;

namespace NaviStudio.WpfApp.Services;


public partial class GMapRouteDisplayService : IGMapRouteDisplayService
{
    #region Public Events

    public event EventHandler? CurrentPositionChanged;

    #endregion Public Events

    #region Public Properties

    public PointLatLng? CurrentPosition => CurrentPositionIndex == -1 ? null : _routeMarkers[CurrentPositionIndex].Position;

    public int CurrentPositionIndex { get; private set; } = -1;

    public double TimeScale { get; set; } = 1;

    #endregion Public Properties

    #region Public Methods

    public void AddPoint(MapPoint point, bool moveToLast = false)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        var marker = CreateRouteMarker(_forwardFill, new() { Lat = point.Latitude, Lng = point.Longitude }, point.TimeStamp);
        _routeMarkers.Add(marker);
        _points.Add(point);
        UpdateClusteredMarkers();
        App.Current.Dispatcher.Invoke(() =>
        {
            if((_routeMarkers.Count - 1) % GetClusterLevel() == 0)
                _gMapControl.Markers.Add(marker);
            if(moveToLast)
                MoveTo(_routeMarkers.Count - 1);
        });
    }

    public void AddPoints(IEnumerable<MapPoint> points, bool moveToLast = false)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        ArgumentNullException.ThrowIfNull(_positionMarker);
        if(!points.Any())
            return;
        foreach(var point in points)
        {
            var marker = CreateRouteMarker(_forwardFill, new() { Lat = point.Latitude, Lng = point.Longitude }, point.TimeStamp);
            _routeMarkers.Add(marker);
            _points.Add(point);
        }
        UpdateClusteredMarkers();
        App.Current.Dispatcher.Invoke(() =>
        {
            Optimize();
            if(moveToLast)
                MoveTo(_routeMarkers.Count - 1);
        });
    }

    public void Clear()
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        _gMapControl.Markers.Clear();
        _positionMarker.Shape.Visibility = Visibility.Collapsed;
        _routeMarkers.Clear();
        _points.Clear();
        foreach(var markers in _clusterLevelToMarkersMap.Values)
            markers.Clear();
        _gMapControl.Bearing = 0;
        CurrentPositionIndex = -1;
    }

    void ThrowIfNoPoints()
    {
        if(_points.Count == 0)
            throw new InvalidOperationException("No points.");
    }

    public void MoveTo(int newIndex)
    {
        ThrowIfNoPoints();
        ArgumentOutOfRangeException.ThrowIfLessThan(newIndex, 0);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(newIndex, _routeMarkers.Count - 1);
        if(CurrentPositionIndex == newIndex)
            return;
        _positionMarker.Shape.Visibility = Visibility.Visible;
        if(CurrentPositionIndex > newIndex)
        {
            for(int i = newIndex + 1; i <= CurrentPositionIndex; i++)
                SetEllipseMarkerFill(_routeMarkers[i], _forwardFill);
        }
        else
        {
            for(int i = int.Max(CurrentPositionIndex, 0); i < newIndex; i++)
                SetEllipseMarkerFill(_routeMarkers[i], _backFill);
        }
        CurrentPositionIndex = newIndex;
        UpdatePositionMarker(_routeMarkers[newIndex].Position);
        CurrentPositionChanged?.Invoke(this, EventArgs.Empty);
    }

    public void MoveToOffset(int offset)
    {
        ThrowIfNoPoints();
        MoveTo(int.Clamp(CurrentPositionIndex + offset, 0, _routeMarkers.Count - 1));
    }

    public IGMapRouteDisplayService RegisterGMapControl(GMapControl gMapControl)
    {
        if(_gMapControl is not null)
            throw new InvalidOperationException("The GMapControl has been registered.");
        _gMapControl = gMapControl;
        SetPositionMarkerShape();
        _gMapControl.Markers.Add(_positionMarker);
        _gMapControl.OnMapZoomChanged += Optimize;
        _gMapControl.OnPositionChanged += (_) => Optimize();
        //_gMapControl.OnMapDrag += Cluster;
        return this;
    }

    public async Task StartAsync(CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        if(_points.Count == 0)
            return;
        ThrowIfIsRunnning();
        _positionMarker.Shape.Visibility = Visibility.Visible;
        _isRunning = true;
        try
        {
            await Task.Run(async () =>
            {
                while(!token.IsCancellationRequested && CurrentPositionIndex < _routeMarkers.Count - 1)
                {
                    var duration = (_points![CurrentPositionIndex + 1].TimeStamp - _points[CurrentPositionIndex].TimeStamp) * TimeScale;
                    await Task.Delay(duration, token);
                    App.Current.Dispatcher.Invoke(() => MoveToOffset(1));
                }
            }, token);
        }
        catch(OperationCanceledException) { }
        finally
        {
            _isRunning = false;
            Trace.WriteLine("Stop");
        }
    }

    #endregion Public Methods

    #region Private Fields

    readonly static Brush _backFill = (Brush)App.Current.Resources["MikuGreenBrush"];

    readonly static Brush _forwardFill = (Brush)App.Current.Resources["MikuRedBrush"];

    readonly List<MapPoint> _points = [];

    readonly GMapMarker _positionMarker = new(new());

    readonly List<GMapMarker> _routeMarkers = [];
    GMapControl? _gMapControl;
    bool _isRunning = false;

    #endregion Private Fields

    // double _groundResolution;

    #region Private Properties

    //PureProjection Projection => _gMapControl!.MapProvider.Projection;

    #endregion Private Properties

    #region Private Methods

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

    #endregion Private Methods

    //void UpdateGroundResolution()
    //{
    //    ArgumentNullException.ThrowIfNull(_gMapControl);
    //    _groundResolution = Projection.GetGroundResolution((int)_gMapControl.Zoom, _gMapControl.Position.Lat);
    //}
}
