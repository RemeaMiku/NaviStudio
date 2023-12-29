using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class ChartGroupPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationEpochDataRecipient(messenger, epochDatasService)
{

    [ObservableProperty]
    string _title = string.Empty;

    [ObservableProperty]
    int _maxEpochCount;

    int _epochCount;

    public Dictionary<ChartPageViewModel, ChartParameters> ChartParas { get; } = [];

    public override void Receive(EpochData message)
    {
        foreach ((var viewModel, var paras) in ChartParas)
        {
            Add(message, viewModel, paras);
            RemoveIfReachMaxEpochCount(viewModel);
        }
    }

    void Add(EpochData epochData, ChartPageViewModel viewModel, ChartParameters paras)
    {
        foreach ((var label, var func) in paras.LabelFuncs)
            viewModel.Add(label, new(epochData.TimeStamp, func(epochData)));
        _epochCount++;
    }

    void RemoveIfReachMaxEpochCount(ChartPageViewModel viewModel)
    {
        var count = _epochCount - MaxEpochCount;
        if (count <= 0)
            return;
        viewModel.Remove(count);
        _epochCount -= count;
    }

    void Clear()
    {
        foreach (var viewModel in ChartParas.Keys)
            viewModel.Clear();
        _epochCount = 0;
    }

    protected override void Reset()
    {
        if (_epochCount > 0)
            Clear();
    }
}
