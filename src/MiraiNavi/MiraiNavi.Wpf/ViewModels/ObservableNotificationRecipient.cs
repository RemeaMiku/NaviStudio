using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Common.Messages;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels;

public abstract class ObservableNotificationRecipient(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableRecipient(messenger), IRecipient<NotificationMessage>
{
    protected override void OnActivated()
    {
        base.OnActivated();
        Sync();
    }

    protected readonly IEpochDatasService _epochDatasService = epochDatasService;

    public virtual void Receive(NotificationMessage message)
    {
        switch (message.Value)
        {
            case Notifications.Reset:
                Reset();
                break;
            case Notifications.Sync:
                Sync();
                break;
            case Notifications.Update:
                Update(_epochDatasService.Last);
                break;
            default:
                break;
        }
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