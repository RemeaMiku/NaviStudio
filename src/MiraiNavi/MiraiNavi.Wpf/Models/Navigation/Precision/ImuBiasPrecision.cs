using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Models.Navigation.Precision;

public record class ImuBiasPrecision
{
    public UtcTime? TimeStamp { get; init; }

    public Vector3 StdAccelerometerBias { get; init; }

    public Vector3 StdGyroscopeBias { get; init; }
}
