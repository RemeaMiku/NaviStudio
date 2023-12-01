using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Models.Navigation;

public record class ImuBiasPrecision
{
    public UtcTime? TimeStamp { get; init; }

    public float StdAccelerometerBiasX { get; init; }

    public float StdAccelerometerBiasY { get; init; }

    public float StdAccelerometerBiasZ { get; init; }

    public float StdGyroscopeBiasX { get; init; }

    public float StdGyroscopeBiasY { get; init; }

    public float StdGyroscopeBiasZ { get; init; }
}
