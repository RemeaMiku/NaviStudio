namespace MiraiNavi.Shared.Models.Chart;

public readonly struct ChartModel(UtcTime timeStamp, double? value)
{
    #region Public Properties

    public DateTime TimeStamp { get; init; } = timeStamp.DateTime;

    public double Value { get; init; } = value ?? double.NaN;

    #endregion Public Properties
}