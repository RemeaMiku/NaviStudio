﻿using NaviSharp;
using NaviSharp.SpatialReference;

namespace MiraiNavi.WpfApp.Models.Navigation;

public record class Pose
{
    public UtcTime? TimeStamp { get; init; }

    public EcefCoord EcefCoord { get; init; }

    public GeodeticCoord GeodeticCoord { get; init; }

    public double EastVelocity { get; init; }

    public double NorthVelocity { get; init; }

    public double UpVelocity { get; init; }

    public double XVelocity { get; init; }

    public double YVelocity { get; init; }

    public double ZVelocity { get; init; }

    public double Velocity => Math.Sqrt(Math.Pow(XVelocity, 2) + Math.Pow(YVelocity, 2) + Math.Pow(ZVelocity, 2));

    public EulerAngles EulerAngles { get; init; }

}
