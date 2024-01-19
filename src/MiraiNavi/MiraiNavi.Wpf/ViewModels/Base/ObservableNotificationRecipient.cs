using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Common.Messaging;
using MiraiNavi.WpfApp.Common.Messaging.Messages;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Base;

public abstract class ObservableNotificationRecipient(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableRecipient(messenger), IRecipient<RealTimeNotification>
{
    protected override void OnActivated()
    {
        base.OnActivated();
        Sync();
    }

    protected readonly IEpochDatasService _epochDatasService = epochDatasService;

    public virtual void Receive(RealTimeNotification message)
    {
        if (message == RealTimeNotification.Update)
            Update(_epochDatasService.Last);
        else if (message == RealTimeNotification.Sync)
            Sync();
        else if (message == RealTimeNotification.Reset)
            Reset();
    }

    protected abstract void Reset();

    protected abstract void Update(EpochData epochData);

    protected virtual void Sync()
    {
        if (_epochDatasService.HasData)
            Update(_epochDatasService.Last);
        else
            Reset();
    }
}