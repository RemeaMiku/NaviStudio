using System.Diagnostics;
using GMap.NET.WindowsPresentation;
using NaviStudio.WpfApp.Common.Extensions;

namespace NaviStudio.WpfApp.Services;

//TODO 地图标记点优化
partial class GMapRouteDisplayService
{
    const int _clusterLevel0 = 1;
    const int _clusterLevel1 = 10;
    const int _clusterLevel2 = 30;
    const int _clusterLevel3 = 100;
    const int _clusterThreshold = 1000;

    readonly Dictionary<int, HashSet<GMapMarker>> _clusterLevelToMarkersMap = [];

    int GetClusterLevel()
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        return _gMapControl.Zoom switch
        {
            >= 13 => _clusterLevel0,
            >= 10 and < 13 => _clusterLevel1,
            >= 5 and < 10 => _clusterLevel2,
            >= 0 and < 5 => _clusterLevel3,
            _ => throw new Exception("Zoom level is out of range."),
        };
        //return _clusterLevel0;
    }

    HashSet<GMapMarker> GetClusteredMarkers(int clusterLevel)
    {
        if(clusterLevel == _clusterLevel0)
            return [.. _routeMarkers];
        if(!_clusterLevelToMarkersMap.ContainsKey(clusterLevel))
            _clusterLevelToMarkersMap.Add(clusterLevel, []);
        var targetCount = _routeMarkers.Count / clusterLevel;
        var clusteredMarkers = _clusterLevelToMarkersMap[clusterLevel];
        if(targetCount == clusteredMarkers.Count)
            return clusteredMarkers;
        for(int i = clusteredMarkers.Count; i < targetCount; i++)
        {
            clusteredMarkers.Add(_routeMarkers[clusterLevel * i]);
            Trace.WriteLine($"Level {clusterLevel}: Add {clusterLevel * i}");
        }
        return clusteredMarkers;
    }

    void FilterVisibleMarkers(HashSet<GMapMarker> markers)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        markers.RemoveWhere(marker => !marker.IsVisible(_gMapControl));
    }

    void EnableMarkers(HashSet<GMapMarker> markers)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        App.Current.Dispatcher.Invoke(() =>
        {
            foreach(var marker in _routeMarkers)
            {
                if(markers.Contains(marker) && !_gMapControl.Markers.Contains(marker))
                    _gMapControl.Markers.Add(marker);
                else if(!markers.Contains(marker))
                    _gMapControl.Markers.Remove(marker);
            }
        });
    }

    DateTime _lastClusterTime = DateTime.MinValue;

    readonly static TimeSpan _clusterInterval = TimeSpan.FromSeconds(0.5);

    readonly object _clusterLock = new();

    void Cluster()
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        if(DateTime.Now - _lastClusterTime <= _clusterInterval || _routeMarkers.Count <= _clusterThreshold)
            return;
        lock(_clusterLock)
        {
            _gMapControl.Markers.Remove(_positionMarker);
            var clusterLevel = GetClusterLevel();
            var markers = GetClusteredMarkers(clusterLevel);
            if(markers.Count >= _clusterThreshold)
                FilterVisibleMarkers(markers);
            EnableMarkers(markers);
            _lastClusterTime = DateTime.Now;
            Trace.WriteLine($"Clustered: {markers.Count}");
            _gMapControl.Markers.Add(_positionMarker);
        }
    }
}
