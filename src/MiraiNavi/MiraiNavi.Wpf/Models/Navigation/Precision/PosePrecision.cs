using NaviSharp;

namespace MiraiNavi.WpfApp.Models.Navigation.Precision;

public record class PosePrecision
{
    public UtcTime? TimeStamp { get; init; }

    public float Ratio { get; init; }

    public float Hdop { get; init; }

    public float Vdop { get; init; }

    public float StdEastVelocity { get; init; }

    public float StdNorthVelocity { get; init; }

    public float StdUpVelocity { get; init; }

    public float StdEastNorthVelocity { get; init; }

    public float StdEastUpVelocity { get; init; }

    public float StdNorthUpVelocity { get; init; }

    public Angle StdYaw { get; init; }

    public Angle StdPitch { get; init; }

    public Angle StdRoll { get; init; }

    public Angle StdYawPitch { get; init; }

    public Angle StdYawRoll { get; init; }

    public Angle StdPitchRoll { get; init; }
}
