using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.Shared.Models.Solution;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class ChartGroupPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationEpochDataRecipient(messenger, epochDatasService)
{

    [ObservableProperty]
    string _title = "图表组";

    [ObservableProperty]
    int _maxEpochCount;

    int _epochCount;

    public Dictionary<ChartPageViewModel, ChartParameters> ChartParas { get; } = [];

    public override void Receive(EpochData message)
    {
        var removeCount = _epochCount - MaxEpochCount + 1;
        foreach ((var viewModel, var paras) in ChartParas)
        {
            if (removeCount > 0)
                viewModel.RemoveOnAllSeries(removeCount);
            AddOnAllSeries(message, viewModel, paras);
        }
        _epochCount = Math.Min(_epochCount + 1, MaxEpochCount);
    }

    protected override void Sync()
    {
        Reset();
        _epochCount = Math.Min(MaxEpochCount, _epochDatasService.Datas.Count);
        foreach (var epochData in _epochDatasService.Datas.TakeLast(MaxEpochCount))
            foreach ((var viewModel, var paras) in ChartParas)
                AddOnAllSeries(epochData, viewModel, paras);
    }

    static void AddOnAllSeries(EpochData epochData, ChartPageViewModel viewModel, ChartParameters paras)
    {
        foreach ((var label, var func) in paras.LabelFuncs)
            viewModel.Add(label, new(epochData.TimeStamp, func(epochData)));
    }

    void RemoveIfReachMaxEpochCount()
    {
        var count = _epochCount - MaxEpochCount;
        if (count <= 0)
            return;
        foreach (var viewModel in ChartParas.Keys)
            viewModel.RemoveOnAllSeries(count);
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
