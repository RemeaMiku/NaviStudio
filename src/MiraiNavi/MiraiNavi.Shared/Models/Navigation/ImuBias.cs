using System.Numerics;

namespace MiraiNavi.Shared.Models;

public record class ImuBias
{
    public UtcTime TimeStamp { get; set; }

    public Vector3 AccelerometerBias { get; set; }

    public Vector3 GyroscopeBias { get; set; }
}
