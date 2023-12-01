using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Models.Navigation;

public record class ImuBias
{
    public UtcTime? TimeStamp { get; init; }

    public Vector3 AccelerometerBias { get; init; }

    public Vector3 GyroscopeBias { get; init; }
}
