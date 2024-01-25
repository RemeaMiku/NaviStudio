using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Common.Messaging.Messages;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Base;

public abstract class ObservableNotificationRecipient(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableRecipient(messenger), IRecipient<RealTimeNotification>
{
    #region Public Methods

    public virtual void Receive(RealTimeNotification message)
    {
        if (message == RealTimeNotification.Update)
            Update(_epochDatasService.Last);
        else if (message == RealTimeNotification.Sync)
            Sync();
        else if (message == RealTimeNotification.Reset)
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
    protected abstract void Reset();

    protected abstract void Update(EpochData epochData);

    protected virtual void Sync()
    {
        if (_epochDatasService.HasData)
            Update(_epochDatasService.Last);
        else
            Reset();
    }

    #endregion Protected Methods
}