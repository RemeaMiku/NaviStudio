using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.Shared.Models.Satellites;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

// TODO 添加系统筛选器
public partial class SatelliteTrackingPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationEpochDataRecipient(messenger, epochDatasService)
{
    public static string Title => "卫星跟踪列表";

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
        SatelliteTrackings = message.SatelliteTrackings;
    }

    protected override void Reset()
    {
        SatelliteTrackings = default;
    }
}
