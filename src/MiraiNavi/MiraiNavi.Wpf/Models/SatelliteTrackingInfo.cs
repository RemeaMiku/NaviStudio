namespace MiraiNavi.WpfApp.Models;

public readonly struct SatelliteTrackingInfo
{
    public SatelliteTrackingInfo(UtcTime? timeStamp, SatelliteInfo satelliteInfo, float azimuth, float elevation)
    {
        TimeStamp = timeStamp ?? UtcTime.Now;
        SatelliteInfo = satelliteInfo;
        Azimuth = azimuth;
        Elevation = elevation;
    }

    public UtcTime TimeStamp { get; init; }

    public SatelliteInfo SatelliteInfo { get; init; }

    /// <summary>
    /// 方位角
    /// </summary>
    public float Azimuth { get; init; }

    /// <summary>
    /// 高度角
    /// </summary>
    public float Elevation { get; init; }
}
