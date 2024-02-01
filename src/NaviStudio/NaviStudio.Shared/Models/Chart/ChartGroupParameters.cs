namespace NaviStudio.Shared.Models.Chart;

public class ChartGroupParameters(string title, int maxEpochCount, IEnumerable<string> items)
{
    #region Public Properties

    public string Title { get; init; } = title;

    public int MaxEpochCount { get; init; } = maxEpochCount;

    public IEnumerable<string> Items { get; init; } = items;

    #endregion Public Properties
}