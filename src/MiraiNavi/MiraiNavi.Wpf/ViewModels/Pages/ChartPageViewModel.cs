using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MiraiNavi.WpfApp.Models.Chart;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class ChartPageViewModel : ObservableObject
{
    public string Title { get; set; } = string.Empty;

    public Dictionary<string, ObservableCollection<ChartModel>> SeriesDatas { get; } = [];

    public event EventHandler<string>? AddSeriesRequested;

    public void RemoveOnAllSeries(int count)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(count, 0);
        App.Current.Dispatcher.Invoke(() =>
        {
            foreach ((_, var datas) in SeriesDatas)
                for (int i = 0; i < count; i++)
                    datas.RemoveAt(0);
        });
    }

    public void Add(string label, ChartModel model)
        => App.Current.Dispatcher.Invoke(() => SeriesDatas[label].Add(model));

    public void Clear()
    {
        App.Current.Dispatcher.Invoke(() =>
        {
            foreach (var datas in SeriesDatas.Values)
                datas.Clear();
        });
    }
}
