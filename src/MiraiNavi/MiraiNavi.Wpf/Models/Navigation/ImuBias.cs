using System.Numerics;

namespace MiraiNavi.WpfApp.Models.Navigation;

public record class ImuBias
{
    public UtcTime? TimeStamp { get; init; }

    public Vector3 AccelerometerBias { get; init; }

    public Vector3 GyroscopeBias { get; init; }
}
