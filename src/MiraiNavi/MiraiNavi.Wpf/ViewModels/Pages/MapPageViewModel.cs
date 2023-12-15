using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GMap.NET;
using MiraiNavi.WpfApp.Common.Extensions;
using MiraiNavi.WpfApp.Common.Messages;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class MapPageViewModel(IEpochDatasService epochDatasService, IGMapRouteDisplayService gMapRouteReplayService) : ObservableRecipient, IRecipient<EpochData>, IRecipient<RealTimeControlMessage>
{
    readonly IEpochDatasService _epochDatasService = epochDatasService;
    readonly IGMapRouteDisplayService _gMapRouteDisplayService = gMapRouteReplayService;

    [ObservableProperty]
    PointLatLng _mapCenter;

    [ObservableProperty]
    bool _keepCenter = true;

    [ObservableProperty]
    bool _isRealTime = false;

    [ObservableProperty]
    int _positionIndex = -1;

    [ObservableProperty]
    int _routePointsCount = 0;

    [RelayCommand]
    void ReturnPosition()
    {
        KeepCenter = true;
        if (_gMapRouteDisplayService.Position.HasValue)
            MapCenter = _gMapRouteDisplayService.Position.Value;
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        if (!IsRealTime)
            return;
        Sync();
    }

    protected override void OnDeactivated()
    {
        base.OnDeactivated();
    }

    public void Receive(EpochData message)
    {
        ArgumentNullException.ThrowIfNull(message.Pose);
        RoutePointsCount++;
        var point = message.Pose.GeodeticCoord.ToPointLatLng();
        if (KeepCenter)
            MapCenter = point;
        _gMapRouteDisplayService.AddPoint(point, message.TimeStamp, true);
    }

    void Sync()
    {
        _gMapRouteDisplayService.AddPoints(_epochDatasService.Datas.Skip(RoutePointsCount).Select(d => (d.Pose!.GeodeticCoord.ToPointLatLng(), d.TimeStamp)), true);
        RoutePointsCount = _epochDatasService.Datas.Count;
        MapCenter = _gMapRouteDisplayService.Position!.Value;
    }

    //[RelayCommand]
    //void Pause()
    //{
    //    _gMapRouteDisplayService.Pause();
    //}

    public void Receive(RealTimeControlMessage message)
    {
        if (message.Mode == RealTimeControlMode.Start)
        {
            _gMapRouteDisplayService.ClearPoints();
            IsRealTime = true;
            KeepCenter = true;
        }
        else if (message.Mode == RealTimeControlMode.Resume)
        {
            KeepCenter = true;
            Sync();
        }
    }
}
