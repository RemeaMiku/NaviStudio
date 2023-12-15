using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IEpochDatasService : IRecipient<RequestMessage<EpochData>>
{
    public ReadOnlyCollection<EpochData> Datas { get; }

    public void Clear();

    public void Update(EpochData epochData, bool notifyUpdate = true);
}
