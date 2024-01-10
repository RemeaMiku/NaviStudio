namespace MiraiNavi.WpfApp.Models;

public class ChartGroupParameters(string title, int maxEpochCount, IEnumerable<string> items)
{
    public string Title { get; init; } = title;

    public int MaxEpochCount { get; init; } = maxEpochCount;

    public IEnumerable<string> Items { get; init; } = items;
}