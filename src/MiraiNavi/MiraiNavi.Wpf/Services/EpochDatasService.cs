using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MiraiNavi.WpfApp.Common;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.Services;

public class EpochDatasService(IMessenger messenger) : IEpochDatasService
{
    private readonly IMessenger _messenger = messenger;

    private readonly List<EpochData> _epochDatas = [];

    public IEnumerable<EpochData> Datas { get => _epochDatas; }

    public void Clear() => _epochDatas.Clear();

    public void Update(EpochData epochData, bool notifyAfterUpdate = true)
    {
        _epochDatas.Add(epochData);
        if (notifyAfterUpdate)
            _messenger.Send(epochData);
    }
}
