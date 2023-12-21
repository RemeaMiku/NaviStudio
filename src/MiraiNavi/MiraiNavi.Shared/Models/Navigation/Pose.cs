using NaviSharp;
using NaviSharp.SpatialReference;

namespace MiraiNavi.Shared.Models;

public record class Pose
{
    public UtcTime TimeStamp { get; set; }

    public EcefCoord EcefCoord { get; set; }

    public GeodeticCoord GeodeticCoord { get; set; }

    public double EastVelocity { get; set; }

    public double NorthVelocity { get; set; }

    public double UpVelocity { get; set; }

    public double XVelocity { get; set; }

    public double YVelocity { get; set; }

    public double ZVelocity { get; set; }

    public double Velocity => Math.Sqrt(Math.Pow(XVelocity, 2) + Math.Pow(YVelocity, 2) + Math.Pow(ZVelocity, 2));

    public EulerAngles EulerAngles { get; set; }

}
