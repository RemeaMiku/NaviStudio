﻿namespace MiraiNavi.WpfApp.Models;

public readonly struct ChartModel(UtcTime timeStamp, double? value)
{
    public DateTime TimeStamp { get; init; } = timeStamp.DateTime;

    public double Value { get; init; } = value ?? double.NaN;
}