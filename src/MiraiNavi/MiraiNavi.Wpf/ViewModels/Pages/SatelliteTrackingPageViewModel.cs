using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.Shared.Models.Satellites;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

// TODO 添加系统筛选器
public partial class SatelliteTrackingPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationRecipient(messenger, epochDatasService)
{
    public const string Title = "卫星跟踪列表";
    public const string MenuItemHeader = $"{Title}(_T)";

    [ObservableProperty]
    List<SatelliteTracking>? _satelliteTrackings;

    protected override void Update(EpochData message)
    {
        if (message.SatelliteTrackings is null)
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
}
