using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.Shared.Models.Solution;
using MiraiNavi.WpfApp.Common.Messages;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels;

public abstract class ObservableNotificationEpochDataRecipient(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableRecipient(messenger), IRecipient<EpochData>, IRecipient<NotificationMessage>
{
    protected readonly IEpochDatasService _epochDatasService = epochDatasService;

    public virtual void Receive(NotificationMessage message)
    {
        switch (message.Type)
        {
            case NotificationType.Reset:
                Reset();
                break;
            case NotificationType.Sync:
                Sync();
                break;
            default:
                break;
        }
    }

    public abstract void Receive(EpochData message);

    protected abstract void Reset();

    protected virtual void Sync()
    {
        if (_epochDatasService.LastestData is not null)
            Receive(_epochDatasService.LastestData);
        else
            Reset();
    }
}