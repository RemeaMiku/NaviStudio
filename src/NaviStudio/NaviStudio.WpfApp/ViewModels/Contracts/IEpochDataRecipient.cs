using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace NaviStudio.WpfApp.ViewModels.Contracts;

public interface IEpochDataRecipient : IRecipient<ValueChangedMessage<EpochData>>
{
    public void Update(EpochData data);

    public new void Receive(ValueChangedMessage<EpochData> message) => Update(message.Value);
}
