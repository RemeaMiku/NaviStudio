using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using NaviStudio.Shared.Models.Satellites;
using NaviStudio.WpfApp.Services.Contracts;
using NaviStudio.WpfApp.ViewModels.Base;

namespace NaviStudio.WpfApp.ViewModels.Pages;

public partial class SatelliteTrackingPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationRecipient(messenger, epochDatasService)
{
    #region Public Fields

    public const string Title = "卫星跟踪列表";
    public const string MenuItemHeader = $"{Title}(_T)";

    #endregion Public Fields

    #region Protected Methods

    protected override void Update(EpochData message)
    {
        if(message.SatelliteTrackings is null)
        {
            Reset();
            return;
        }
        SatelliteTrackings = message.SatelliteTrackings;
    }

    protected override void Reset()
    {
        SatelliteTrackings = default;
    }

    #endregion Protected Methods

    #region Private Fields

    [ObservableProperty]
    List<SatelliteTracking>? _satelliteTrackings;

    #endregion Private Fields
}
