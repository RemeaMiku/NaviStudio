using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using GMap.NET;
using NaviStudio.Shared.Models.Map;
using NaviStudio.WpfApp.Common.Extensions;
using NaviStudio.WpfApp.Common.Messaging;
using NaviStudio.WpfApp.Services.Contracts;
using NaviStudio.WpfApp.ViewModels.Base;

namespace NaviStudio.WpfApp.ViewModels.Pages;

public partial class MapPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService, IGMapRouteDisplayService gMapRouteReplayService) : ObservableNotificationRecipient(messenger, epochDatasService)
{
    #region Public Fields

    public const string Title = "地图";
    public const string MenuItemHeader = $"{Title}(_M)";

    #endregion Public Fields

    #region Protected Methods

    protected override void Update(EpochData data)
    {
        if(data.Result is null)
            return;
        RoutePointsCount++;
        var point = data.Result.GeodeticCoord.ToPointLatLng();
        _gMapRouteDisplayService.AddPoint(point, data.TimeStamp, true);
    }

    protected override void Sync()
    {
        if(!_epochDatasService.HasData)
        {
            Reset();
            return;
        }
        if(_epochDatasService.EpochCount == RoutePointsCount + 1)
        {
            Update(_epochDatasService.Last);
            return;
        }
        _gMapRouteDisplayService.AddPoints(_epochDatasService.Datas.Skip(RoutePointsCount).Select(d => (d.Result!.GeodeticCoord.ToPointLatLng(), d.TimeStamp)), true);
        RoutePointsCount = _epochDatasService.EpochCount;
    }

    protected override void Reset()
    {
        _gMapRouteDisplayService.Clear();
        //IsRealTime = true;
        KeepCenter = true;
    }

    #endregion Protected Methods

    #region Private Fields

    public NavigationIndicators Indicator
    {
        get => _gMapRouteDisplayService.Indicator;
        set
        {
            if(_gMapRouteDisplayService.Indicator == value)
                return;
            _gMapRouteDisplayService.Indicator = value;
            OnPropertyChanged(nameof(Indicator));
        }
    }

    public bool KeepCenter
    {
        get => _gMapRouteDisplayService.KeepCenter;
        set
        {
            if(_gMapRouteDisplayService.KeepCenter == value)
                return;
            _gMapRouteDisplayService.KeepCenter = value;
            OnPropertyChanged(nameof(KeepCenter));
        }
    }

    public bool EnableMapBearing
    {
        get => _gMapRouteDisplayService.EnableMapBearing;
        set
        {
            if(_gMapRouteDisplayService.EnableMapBearing == value)
                return;
            _gMapRouteDisplayService.EnableMapBearing = value;
            OnPropertyChanged(nameof(EnableMapBearing));
        }
    }

    readonly IGMapRouteDisplayService _gMapRouteDisplayService = gMapRouteReplayService;

    [ObservableProperty]
    PointLatLng _mapCenter;

    [ObservableProperty]
    bool _isRealTime = false;

    [ObservableProperty]
    int _positionIndex;

    [ObservableProperty]
    int _routePointsCount;

    [ObservableProperty]
    double _timeScale = 1;

    [ObservableProperty]
    TimePointLatLng? _selectedPoint;

    #endregion Private Fields

    #region Private Methods

    partial void OnSelectedPointChanged(TimePointLatLng? value)
    {
        var epochData = !value.HasValue ? default : _epochDatasService.GetByTimeStamp(value.Value.Item1);
        Messenger.Send(new ValueChangedMessage<EpochData?>(epochData), MessageTokens.MapPageToPropertyPage);
    }

    [RelayCommand]
    void ReturnToPosition()
    {
        KeepCenter = true;
        if(_gMapRouteDisplayService.CurrentPosition.HasValue)
            MapCenter = _gMapRouteDisplayService.CurrentPosition.Value;
    }

    #endregion Private Methods

    //[RelayCommand]
    //void Pause()
    //{
    //    _gMapRouteDisplayService.Pause();
    //}
}
