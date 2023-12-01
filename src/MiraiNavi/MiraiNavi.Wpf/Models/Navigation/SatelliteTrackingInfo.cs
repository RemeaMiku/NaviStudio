namespace MiraiNavi.WpfApp.Models.Navigation;

public class SatelliteTrackingInfo
{
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
