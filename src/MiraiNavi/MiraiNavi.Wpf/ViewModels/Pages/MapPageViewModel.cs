using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using GMap.NET;
using MiraiNavi.WpfApp.Common.Extensions;
using MiraiNavi.WpfApp.Services.Contracts;
using MiraiNavi.WpfApp.ViewModels.Base;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class MapPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService, IGMapRouteDisplayService gMapRouteReplayService) : ObservableNotificationRecipient(messenger, epochDatasService)
{
    public const string Title = "地图";
    public const string MenuItemHeader = $"{Title}(_M)";

    readonly IGMapRouteDisplayService _gMapRouteDisplayService = gMapRouteReplayService;

    [ObservableProperty]
    PointLatLng _mapCenter;

    [ObservableProperty]
    bool _keepCenter = true;

    //[ObservableProperty]
    //bool _isRealTime = false;

    [ObservableProperty]
    int _positionIndex = -1;

    [ObservableProperty]
    int _routePointsCount = 0;

    [ObservableProperty]
    TimePointLatLng? _selectedPoint;

    partial void OnSelectedPointChanged(TimePointLatLng? value)
    {
        var epochData = !value.HasValue ? default : _epochDatasService.GetByTimeStamp(value.Value.Item1);
        Messenger.Send(new ValueChangedMessage<EpochData?>(epochData), Title);
    }

    [RelayCommand]
    void ReturnToPosition()
    {
        KeepCenter = true;
        if (_gMapRouteDisplayService.CurrentPosition.HasValue)
            MapCenter = _gMapRouteDisplayService.CurrentPosition.Value;
    }

    protected override void Update(EpochData data)
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
        if (!_epochDatasService.HasData)
        {
            Reset();
            return;
        }
        if (_epochDatasService.EpochCount == RoutePointsCount + 1)
        {
            Update(_epochDatasService.Last);
            return;
        }
        _gMapRouteDisplayService.AddPoints(_epochDatasService.Datas.Skip(RoutePointsCount).Select(d => (d.Result!.GeodeticCoord.ToPointLatLng(), d.TimeStamp)), true);
        RoutePointsCount = _epochDatasService.EpochCount;
        if (KeepCenter && _gMapRouteDisplayService.CurrentPosition.HasValue)
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
        //IsRealTime = true;
        KeepCenter = true;
    }
}
