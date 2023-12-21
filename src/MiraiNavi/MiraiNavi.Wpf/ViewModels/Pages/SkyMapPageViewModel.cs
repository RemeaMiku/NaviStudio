using System.Collections.ObjectModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

//TODO 高度角和系统筛选器
public partial class SkyMapPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationEpochDataRecipient(messenger, epochDatasService)
{
    public CollectionViewSource ViewSource { get; } = new();

    public override void Receive(EpochData message)
    {
        ViewSource.Source = message.SatelliteSkyPositions;
    }

    protected override void Reset()
    {
        ViewSource.Source = default;
    }
}
