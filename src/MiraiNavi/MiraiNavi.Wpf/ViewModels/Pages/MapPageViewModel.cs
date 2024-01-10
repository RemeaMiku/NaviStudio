using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GMap.NET;
using MiraiNavi.Shared.Models.Solution;
using MiraiNavi.WpfApp.Common.Extensions;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class MapPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService, IGMapRouteDisplayService gMapRouteReplayService) : ObservableNotificationEpochDataRecipient(messenger, epochDatasService)
{
    public const string Title = "地图";
    public const string MenuItemHeader = $"{Title}(_M)";

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

    [ObservableProperty]
    TimePointLatLng _selectedPoint;

    partial void OnSelectedPointChanged(TimePointLatLng value)
    {
        (var timeStamp, var _) = SelectedPoint;
        Messenger.Send(_epochDatasService.GetEpochDataByTimeStamp(timeStamp), Title);
    }

    [RelayCommand]
    void ReturnToPosition()
    {
        KeepCenter = true;
        if (_gMapRouteDisplayService.CurrentPosition.HasValue)
            MapCenter = _gMapRouteDisplayService.CurrentPosition.Value;
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        if (!IsRealTime)
            return;
        Sync();
    }

    public override void Receive(EpochData data)
    {
        if (data.Result is null)
            return;
        RoutePointsCount++;
        var point = data.Result.GeodeticCoord.ToPointLatLng();
        if (KeepCenter)
            MapCenter = point;
        _gMapRouteDisplayService.AddPoint(point, data.TimeStamp, true);
    }

    protected override void Sync()
    {
        KeepCenter = true;
        _gMapRouteDisplayService.AddPoints(_epochDatasService.Datas.Skip(RoutePointsCount).Select(d => (d.Result!.GeodeticCoord.ToPointLatLng(), d.TimeStamp)), true);
        RoutePointsCount = _epochDatasService.Datas.Count;
        if (_gMapRouteDisplayService.CurrentPosition.HasValue)
            MapCenter = _gMapRouteDisplayService.CurrentPosition.Value;
    }

    //[RelayCommand]
    //void Pause()
    //{
    //    _gMapRouteDisplayService.Pause();
    //}

    protected override void Reset()
    {
        _gMapRouteDisplayService.Clear();
        IsRealTime = true;
        KeepCenter = true;
    }
}
