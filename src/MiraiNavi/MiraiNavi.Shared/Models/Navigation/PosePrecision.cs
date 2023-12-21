using NaviSharp;

namespace MiraiNavi.Shared.Models;

public record class PosePrecision
{
    public UtcTime TimeStamp { get; set; }

    public float Ratio { get; set; }

    public float Hdop { get; set; }

    public float Vdop { get; set; }

    public float StdEastVelocity { get; set; }

    public float StdNorthVelocity { get; set; }

    public float StdUpVelocity { get; set; }

    public float StdEastNorthVelocity { get; set; }

    public float StdEastUpVelocity { get; set; }

    public float StdNorthUpVelocity { get; set; }

    public Angle StdYaw { get; set; }

    public Angle StdPitch { get; set; }

    public Angle StdRoll { get; set; }

    public Angle StdYawPitch { get; set; }

    public Angle StdYawRoll { get; set; }

    public Angle StdPitchRoll { get; set; }
}
