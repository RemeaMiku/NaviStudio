using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MiraiNavi.WpfApp.Common.Messages;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Models.Navigation;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class PosePageViewModel(IMessenger messenger) : ObservableRecipient(messenger), IRecipient<EpochData>, IRecipient<RealTimeControlMessage>
{
    [ObservableProperty]
    Pose? _pose;

    protected override void OnActivated()
    {
        base.OnActivated();
        var message = Messenger.Send(new RequestMessage<EpochData>());
        if (message.HasReceivedResponse)
            Receive(message);
        else
            Pose = default;
    }

    public void Receive(EpochData message) => Pose = message.Pose;

    public void Receive(RealTimeControlMessage message)
    {
        if (message.Mode == RealTimeControlMode.Start)
            Pose = default;
    }
}
