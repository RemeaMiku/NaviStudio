using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Common.Helpers;
using MiraiNavi.WpfApp.Services.Contracts;
using MiraiNavi.WpfApp.ViewModels.Base;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class ChartGroupPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationRecipient(messenger, epochDatasService)
{

    [ObservableProperty]
    string _title = string.Empty;

    [ObservableProperty]
    int _maxEpochCount;

    int _epochCount;

    public HashSet<ChartPageViewModel> ItemViewModels { get; } = [];

    protected override void Update(EpochData epochData)
    {
        var removeCount = _epochCount - MaxEpochCount + 1;
        foreach (var viewModel in ItemViewModels)
        {
            if (removeCount > 0)
                viewModel.RemoveOnAllSeries(removeCount);
            if (ChartItemManager.ChartItemFuncs.ContainsKey(viewModel.Title))
                UpdateChartItemCommon(viewModel, epochData);

        }
        _epochCount = Math.Min(_epochCount + 1, MaxEpochCount);
    }

    static void UpdateChartItemCommon(ChartPageViewModel viewModel, EpochData epochData)
    {
        var funcs = ChartItemManager.ChartItemFuncs[viewModel.Title];
        foreach ((var label, var func) in funcs)
            viewModel.Add(label, new(epochData.TimeStamp, func(epochData)));
    }

    protected override void Sync()
    {
        Reset();
        if (!_epochDatasService.HasData)
            return;
        _epochCount = Math.Min(MaxEpochCount, _epochDatasService.EpochCount);
        foreach (var epochData in _epochDatasService.Datas.TakeLast(MaxEpochCount))
            foreach (var viewModel in ItemViewModels)
                UpdateChartItemCommon(viewModel, epochData);
    }

    protected override void Reset()
    {
        if (_epochCount == 0)
            return;
        foreach (var viewModel in ItemViewModels)
            viewModel.Clear();
        _epochCount = 0;
    }
}
