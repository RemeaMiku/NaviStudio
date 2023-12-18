using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IEpochDatasService
{
    public ReadOnlyCollection<EpochData> Datas { get; }

    public EpochData? LastestData => Datas.Count == 0 ? default : Datas[^1];

    public void Clear();

    public void Add(EpochData epochData);
}
