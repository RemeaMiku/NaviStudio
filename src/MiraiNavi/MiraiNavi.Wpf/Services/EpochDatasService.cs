using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public ReadOnlyCollection<EpochData> Datas => _epochDatas.AsReadOnly();

    public void Clear()
    {
        if (_epochDatas.Count == 0)
            return;
        _epochDatas.Clear();
        _messenger.Unregister<RequestMessage<EpochData>>(this);
    }

    public void Receive(RequestMessage<EpochData> message)
    {
        if (_epochDatas.Count == 0)
            return;
        message.Reply(_epochDatas[^1]);
    }

    public void Update(EpochData epochData, bool notifyUpdate = true)
    {
        if (_epochDatas.Count == 0)
            _messenger.Register(this);
        _epochDatas.Add(epochData);
        if (notifyUpdate)
            _messenger.Send(epochData);
    }
}
