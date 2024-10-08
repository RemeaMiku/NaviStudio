﻿using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using NaviStudio.WpfApp.Common.Extensions;

namespace NaviStudio.WpfApp.Services;

partial class GMapRouteDisplayService
{
    const int _clusterLevel0 = 1;
    const int _clusterLevel1 = 5;
    const int _clusterLevel2 = 15;
    const int _clusterLevel3 = 50;
    const int _clusterLevel4 = 500;
    const int _clusterThreshold = 1000;

    readonly Dictionary<int, HashSet<GMapMarker>> _clusterLevelToMarkersMap = new()
       {
           { _clusterLevel1, new() },
           { _clusterLevel2, new() },
           { _clusterLevel3, new() },
           { _clusterLevel4, new() },
       };

    int GetClusterLevel()
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        return _gMapControl.Zoom switch
        {
            >= 16 => _clusterLevel0,
            >= 14 and < 16 => _clusterLevel1,
            >= 10 and < 14 => _clusterLevel2,
            >= 5 and < 10 => _clusterLevel3,
            >= 0 and < 5 => _clusterLevel4,
            _ => throw new Exception("Zoom level is out of range."),
        };
    }

    void UpdateClusteredMarkers()
    {
        foreach((var level, var markers) in _clusterLevelToMarkersMap)
            for(var i = markers.Count * level; i < _routeMarkers.Count; i += level)
                markers.Add(_routeMarkers[i]);
    }

    HashSet<GMapMarker> GetClusteredMarkers(int level)
    {
        if(level == _clusterLevel0 || _routeMarkers.Count <= _clusterThreshold)
            return [.. _routeMarkers];
        if(!_clusterLevelToMarkersMap.ContainsKey(level))
            _clusterLevelToMarkersMap.Add(level, []);
        var markers = _clusterLevelToMarkersMap[level];
        return [.. markers];
    }

    void EnableMarkers(HashSet<GMapMarker> markers)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        App.Current.Dispatcher.Invoke(() =>
        {
            foreach(var marker in _gMapControl.Markers.Except(markers).ToHashSet())
                if(_routeMarkers.Contains(marker))
                    _gMapControl.Markers.Remove(marker);
            foreach(var marker in markers.Except(_gMapControl.Markers).ToHashSet())
                _gMapControl.Markers.Add(marker);
        });
    }

    readonly object _clusterLock = new();

    void Optimize()
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        var watch = new Stopwatch();
        watch.Start();
        _gMapControl.Markers.Remove(_positionMarker);
        var clusterLevel = GetClusterLevel();
        var markers = GetClusteredMarkers(clusterLevel);
        Trace.WriteLine($"GetClusteredMarkers : {watch.ElapsedMilliseconds}ms");
        if(markers.Count >= _clusterThreshold)
            markers.RemoveWhere(marker => !marker.IsVisible(_gMapControl));
        Trace.WriteLine($"FilterVisibleMarkers : {watch.ElapsedMilliseconds}ms");
        EnableMarkers(markers);
        Trace.WriteLine($"EnableMarkers : {watch.ElapsedMilliseconds}ms");
        Trace.WriteLine($"Zoom:{_gMapControl.Zoom} Level:{clusterLevel} Count: {markers.Count} Time:{watch.ElapsedMilliseconds}ms");
        _gMapControl.Markers.Add(_positionMarker);
        watch.Reset();
    }
}
