using MiraiNavi.Shared.Models.Satellites;

namespace MiraiNavi.Shared.Models;

public record class EpochData
{
    public UtcTime TimeStamp { get; set; }

    public Pose? Pose { get; set; }

    public double? EastBaseLine { get; set; }

    public double? NorthBaseLine { get; set; }

    public double? UpBaseLine { get; set; }

    public ImuBias? ImuBias { get; set; }

    public PosePrecision? PosePrecision { get; set; }

    public BaseLinePrecision? BaseLinePrecision { get; set; }

    public ImuBiasPrecision? ImuBiasPrecision { get; set; }

    public HashSet<Satellite>? Satellites { get; set; }

    public List<SatelliteSkyPosition>? SatelliteSkyPositions { get; set; }

    public List<SatelliteSignalNoiseRatio>? SatelliteSignalNoiseRatios { get; set; }
}
