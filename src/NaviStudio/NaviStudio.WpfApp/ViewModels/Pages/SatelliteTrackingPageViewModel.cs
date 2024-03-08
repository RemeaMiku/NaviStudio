using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using NaviStudio.Shared.Models.Satellites;
using NaviStudio.WpfApp.Services.Contracts;
using NaviStudio.WpfApp.ViewModels.Base;

namespace NaviStudio.WpfApp.ViewModels.Pages;

public partial class SatelliteTrackingPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableSingleEpochDataRecipient(messenger, epochDatasService)
{
    #region Public Fields

    public const string Title = "卫星跟踪列表";
    public const string MenuItemHeader = $"{Title}(_T)";

    #endregion Public Fields    

    readonly static TimeSpan _minInterval = TimeSpan.FromSeconds(3);

    readonly Timer _timer = new()
    {
        Interval = (_minInterval / 10).TotalMilliseconds,
    };
    UtcTime _lastUpdatedTime = UtcTime.MinValue;
    EpochData? _cachedEpochData;

    #region Protected Methods

    public override void Update(EpochData message)
    {
        _cachedEpochData = message;
        var currentTime = UtcTime.UtcNow;
        if(currentTime - _lastUpdatedTime < _minInterval)
        {
            if(_timer.Enabled)
                return;
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
            return;
        }
        _lastUpdatedTime = currentTime;
        UpdateInternal(message);
    }

    void UpdateInternal(EpochData data)
    {
        if(data.SatelliteTrackings is null)
        {
            Reset();
            return;
        }
        SatelliteTrackings = data.SatelliteTrackings;
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        if(e.SignalTime - _lastUpdatedTime < _minInterval)
            return;
        UpdateInternal(_cachedEpochData!);
        _timer.Elapsed -= OnTimerElapsed;
        _timer.Stop();
    }

    public override void Reset()
    {
        SatelliteTrackings = default;
    }

    #endregion Protected Methods

    #region Private Fields

    [ObservableProperty]
    List<SatelliteTracking>? _satelliteTrackings;

    #endregion Private Fields
}
