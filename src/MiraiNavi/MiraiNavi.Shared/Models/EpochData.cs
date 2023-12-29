using MiraiNavi.Shared.Models.Satellites;

namespace MiraiNavi.Shared.Models;

public record class EpochData
{
    public UtcTime TimeStamp { get; set; }

    public Pose? Pose { get; set; }

    public double? EastLocalPosition { get; set; }

    public double? NorthLocalPosition { get; set; }

    public double? UpLocalPosition { get; set; }

    public ImuBias? ImuBias { get; set; }

    public PosePrecision? PosePrecision { get; set; }

    public LocalPositionPrecision? LocalPositionPrecision { get; set; }

    public ImuBiasPrecision? ImuBiasPrecision { get; set; }

    public HashSet<Satellite>? Satellites { get; set; }

    public List<SatelliteSkyPosition>? SatelliteSkyPositions { get; set; }

    public List<SatelliteTracking>? SatelliteTrackings { get; set; }

    public float Ratio { get; set; }

    public float Hdop { get; set; }

    public float Vdop { get; set; }

    public float Pdop { get; set; }

    public float Gdop { get; set; }

}
