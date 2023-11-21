namespace MiraiNavi.WpfApp.Models;

public readonly struct SatelliteTrackingInfo(UtcTime? timeStamp, Satellite satellite, float azimuth, float elevation)
{
    public UtcTime TimeStamp { get; init; } = timeStamp ?? UtcTime.Now;

    public Satellite Satellite { get; init; } = satellite;

    /// <summary>
    /// 方位角
    /// </summary>
    public float Azimuth { get; init; } = azimuth;

    /// <summary>
    /// 高度角
    /// </summary>
    public float Elevation { get; init; } = elevation;
}
