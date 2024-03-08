using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using NaviStudio.WpfApp.Common.Messaging.Messages;
using NaviStudio.WpfApp.Services.Contracts;

namespace NaviStudio.WpfApp.ViewModels.Base;

public abstract class ObservableNotificationRecipient(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableRecipient(messenger), IRecipient<NotificationMessage>
{
    #region Public Methods

    public virtual void Receive(NotificationMessage message)
    {
        if(message == NotificationMessage.Update)
            Update(_epochDatasService.Last);
        else if(message == NotificationMessage.Sync)
            Sync();
        else if(message == NotificationMessage.Reset)
            Reset();
    }

    #endregion Public Methods

    #region Protected Fields

    protected readonly IEpochDatasService _epochDatasService = epochDatasService;

    #endregion Protected Fields

    #region Protected Methods

    protected override void OnActivated()
    {
        base.OnActivated();
        Sync();
    }
    public abstract void Reset();

    public abstract void Update(EpochData epochData);

    public virtual void Sync()
    {
        if(_epochDatasService.HasData)
            Update(_epochDatasService.Last);
        else
            Reset();
    }

    //public virtual void Receive(ValueChangedMessage<EpochData> message)
    //{
    //    Update(message.Value);
    //}

    #endregion Protected Methods
}