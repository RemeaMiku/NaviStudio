using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Models.Navigation.Precision;

public record class BaseLinePrecision
{
    public UtcTime? TimeStamp { get; init; }

    public float StdEast { get; init; }

    public float StdNorth { get; init; }

    public float StdUp { get; init; }

    public float StdEastNorth { get; init; }

    public float StdEastUp { get; init; }

    public float StdNorthUp { get; init; }
}
