﻿using NaviSharp;

namespace NaviStudio.Shared.Models.Satellites;

public record struct SatelliteSkyPosition
{
    Angle _azimuth;
    Angle _elevation;

    public Satellite Satellite { get; set; }

    /// <summary>
    /// 方位角
    /// </summary>
    public Angle Azimuth
    {
        readonly get => _azimuth;
        set => _azimuth = Angle.Clamp(value, Angle.ZeroAngle, Angle.RoundAngle);
    }

    /// <summary>
    /// 高度角
    /// </summary>
    public Angle Elevation
    {
        readonly get => _elevation;
        set => _elevation = Angle.Clamp(value, Angle.ZeroAngle, Angle.RightAngle);
    }

    public readonly double AzimuthDegrees => Azimuth.Degrees;

    public readonly double ElevationDegrees => Elevation.Degrees;

    public override readonly string ToString() => $"{Satellite}: {Azimuth:F1}°, {Elevation:F1}°";
}
