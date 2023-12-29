using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Models;

public readonly struct ChartModel(UtcTime timeStamp, double? value)
{
    public DateTime TimeStamp { get; init; } = timeStamp.DateTime;

    public double Value { get; init; } = value ?? double.NaN;
}