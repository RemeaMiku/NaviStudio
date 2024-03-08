using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using NaviStudio.WpfApp.Services.Contracts;

namespace NaviStudio.WpfApp.ViewModels.Base;

public abstract class ObservableSingleEpochDataRecipient(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationRecipient(messenger, epochDatasService), IRecipient<ValueChangedMessage<EpochData>>
{
    public void Receive(ValueChangedMessage<EpochData> message)
    {
        Update(message.Value);
    }
}
