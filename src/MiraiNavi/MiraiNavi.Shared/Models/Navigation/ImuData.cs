using System.Numerics;

namespace MiraiNavi.Shared.Models;

public record class ImuData
{
    public UtcTime? TimeStamp { get; init; }

    public Vector3 Acceleration { get; init; }

    public Vector3 AngularVelocity { get; init; }

    public TimeSpan Duration { get; init; }
}
