using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NaviSharp;
using NaviSharp.Orientation;
using NaviSharp.SpatialReference;
using NaviSharp.Time;

namespace MiraiNavi.WpfApp.Models.Navigation;

public class NavigationParameters
{
    public UtcTime? TimeStamp { get; init; }

    public EcefCoord EcefCoord { get; init; }

    public GeodeticCoord GeodeticCoord { get; init; }

    public float EastVelocity { get; init; }

    public float NorthVelocity { get; init; }

    public float UpVelocity { get; init; }

    public Vector3 EcefVelocity { get; init; }

    public float Velocity => EcefVelocity.LengthSquared();

    public EulerAngles EulerAngles { get; init; }

}
