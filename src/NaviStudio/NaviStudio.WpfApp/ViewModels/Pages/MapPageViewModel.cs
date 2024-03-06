using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using GMap.NET;
using NaviStudio.Shared.Models.Map;
using NaviStudio.WpfApp.Common.Extensions;
using NaviStudio.WpfApp.Common.Messaging;
using NaviStudio.WpfApp.Common.Messaging.Messages;
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

    protected override void OnActivated()
    {
        base.OnActivated();
        if(!Messenger.IsRegistered<ValueChangedMessage<bool>, string>(this, MessageTokens.IsRealTimeRunning))
            Messenger.Register(this, MessageTokens.IsRealTimeRunning, (MapPageViewModel r, ValueChangedMessage<bool> m) => r.IsReplayAvailable = !m.Value);
        _gMapRouteDisplayService.CurrentPositionChanged += (_, _) => PositionIndex = _gMapRouteDisplayService.CurrentPositionIndex;
    }

    protected override void OnDeactivated()
    {
        Messenger.Unregister<RealTimeNotification>(this);
    }

    protected override void Update(EpochData data)
    {
        if(data.Result is null)
            return;
        if(!IsRealTime)
            return;
        _gMapRouteDisplayService.AddPoint(MapPoint.FromEpochData(data), true);
        RoutePointsCount++;
        PositionIndex = MaxPositionIndex;
    }

    protected override void Sync()
    {
        IsRealTime = true;
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
        _gMapRouteDisplayService.AddPoints(_epochDatasService.Datas.Skip(RoutePointsCount).Select(MapPoint.FromEpochData), true);
        RoutePointsCount = _epochDatasService.EpochCount;
        PositionIndex = MaxPositionIndex;
    }

    protected override void Reset()
    {
        _gMapRouteDisplayService.Clear();
        RoutePointsCount = 0;
        IsRealTime = true;
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
    bool _isRealTime;

    [ObservableProperty]
    int _positionIndex;

    [ObservableProperty]
    bool _isReplaying;

    [ObservableProperty]
    bool _isReplayAvailable = true;

    public int StepLength => int.Max(RoutePointsCount / 100, 1);

    public int MaxPositionIndex => RoutePointsCount - 1;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StepLength))]
    [NotifyPropertyChangedFor(nameof(MaxPositionIndex))]
    int _routePointsCount;

    //[ObservableProperty]
    //double _timeScale = 1;

    public double TimeScale
    {
        get => _gMapRouteDisplayService.TimeScale;
        set
        {
            if(_gMapRouteDisplayService.TimeScale == value)
                return;
            _gMapRouteDisplayService.TimeScale = value;
            OnPropertyChanged(nameof(TimeScale));
        }
    }

    [ObservableProperty]
    TimePointLatLng? _selectedPoint;

    #endregion Private Fields

    #region Private Methods

    partial void OnSelectedPointChanged(TimePointLatLng? value)
    {
        var epochData = !value.HasValue ? default : _epochDatasService.GetByTimeStamp(value.Value.Item1);
        Messenger.Send(new ValueChangedMessage<EpochData?>(epochData), MessageTokens.MapSelectionChanged);
    }

    [RelayCommand]
    void ReturnToPosition()
    {
        KeepCenter = true;
        if(_gMapRouteDisplayService.CurrentPosition.HasValue)
            MapCenter = _gMapRouteDisplayService.CurrentPosition.Value;
    }

    CancellationTokenSource? _tokenSource;

    [RelayCommand]
    async Task Start()
    {
        if(IsReplaying)
            return;
        _tokenSource = new();
        IsReplaying = true;
        await _gMapRouteDisplayService.StartAsync(_tokenSource.Token);
        IsReplaying = false;
    }

    [RelayCommand]
    void Pause()
    {
        if(!IsReplaying)
            return;
        ArgumentNullException.ThrowIfNull(_tokenSource);
        _tokenSource.Cancel();
        IsReplaying = false;
        Thread.Sleep(100);
    }

    [RelayCommand]
    void MoveForward()
    {
        Pause();
        _gMapRouteDisplayService.MoveToOffset(StepLength);
    }

    [RelayCommand]
    void MoveBackward()
    {
        Pause();
        _gMapRouteDisplayService.MoveToOffset(-StepLength);
    }

    partial void OnIsRealTimeChanged(bool value)
    {
        if(IsRealTime)
        {
            Pause();
            //Sync();
        }
    }

    partial void OnPositionIndexChanged(int value)
    {
        if(IsRealTime)
            return;
        _gMapRouteDisplayService.MoveTo(value);
    }

    #endregion Private Methods

    //[RelayCommand]
    //void Pause()
    //{
    //    _gMapRouteDisplayService.Pause();
    //}
}
