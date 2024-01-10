using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.Shared.Models.Satellites;
using MiraiNavi.Shared.Models.Solution;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

// TODO 添加系统筛选器
public partial class SatelliteTrackingPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationEpochDataRecipient(messenger, epochDatasService)
{
    public const string Title = "卫星跟踪列表";
    public const string MenuItemHeader = $"{Title}(_T)";

    [ObservableProperty]
    List<SatelliteTracking>? _satelliteTrackings;

    public override void Receive(EpochData message)
    {
        if (message.SatelliteTrackings is null)
        {
            Reset();
            Messenger.Send(new Output(UtcTime.Now, Title, InfoBarSeverity.Warning, "卫星跟踪数据异常"));
            return;
        }
        SatelliteTrackings = message.SatelliteTrackings.ToList();
    }

    protected override void Reset()
    {
        SatelliteTrackings = default;
    }
}
