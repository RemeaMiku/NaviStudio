namespace MiraiNavi.Shared.Models;

public record class SatelliteTrackingInfo
{
    public SatelliteTrackingInfo(Satellite satellite)
    {
        Satellite = satellite;
    }

    public UtcTime? TimeStamp { get; init; }

    public Satellite Satellite { get; init; }

    /// <summary>
    /// 方位角
    /// </summary>
    public float Azimuth { get; init; }

    /// <summary>
    /// 高度角
    /// </summary>
    public float Elevation { get; init; }

    public float SignalNoiseRatio { get; init; }

}
