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

public partial class MapPageViewModel(IEpochDatasService epochDatasService, IGMapRouteDisplayService gMapRouteReplayService) : ObservableRecipient, IRecipient<EpochData>, IRecipient<GlobalControlMessage>
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

    protected override void OnActivated()
    {
        base.OnActivated();
        if (!IsRealTime)
            return;
        RoutePointsCount = _epochDatasService.Datas.Count();
        _gMapRouteDisplayService.AddPoints(_epochDatasService.Datas.Select(d => (d.Pose!.GeodeticCoord.ToPointLatLng(), d.TimeStamp)), true);
    }

    protected override void OnDeactivated()
    {
        base.OnDeactivated();
    }

    public void Receive(EpochData message)
    {
        ArgumentNullException.ThrowIfNull(message.Pose);
        RoutePointsCount++;
        PositionIndex++;
        var point = message.Pose.GeodeticCoord.ToPointLatLng();
        if (KeepCenter)
            MapCenter = point;
        _gMapRouteDisplayService.AddPoint(point, message.TimeStamp, true);
    }

    //[RelayCommand]
    //void Pause()
    //{
    //    _gMapRouteDisplayService.Pause();
    //}

    public void Receive(GlobalControlMessage message)
    {
        IsRealTime = message.Mode == GlobalControlMessage.ControlMode.Start;
    }
}
