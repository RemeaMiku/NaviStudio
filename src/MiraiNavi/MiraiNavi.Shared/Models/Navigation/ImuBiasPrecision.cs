using System.Numerics;

namespace MiraiNavi.Shared.Models;

public record class ImuBiasPrecision
{
    public UtcTime TimeStamp { get; set; }

    public Vector3 StdAccelerometerBias { get; set; }

    public Vector3 StdGyroscopeBias { get; set; }
}
