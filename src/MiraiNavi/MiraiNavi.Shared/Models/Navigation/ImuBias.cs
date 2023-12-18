using System.Numerics;

namespace MiraiNavi.Shared.Models;

public record class ImuBias
{
    public UtcTime? TimeStamp { get; init; }

    public Vector3 AccelerometerBias { get; init; }

    public Vector3 GyroscopeBias { get; init; }
}
