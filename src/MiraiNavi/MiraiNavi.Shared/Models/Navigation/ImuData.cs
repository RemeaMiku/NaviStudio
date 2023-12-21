using System.Numerics;

namespace MiraiNavi.Shared.Models;

public record class ImuData
{
    public UtcTime TimeStamp { get; set; }

    public Vector3 Acceleration { get; set; }

    public Vector3 AngularVelocity { get; set; }

    public TimeSpan Duration { get; set; }
}
