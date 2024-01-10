using MiraiNavi.Shared.Models.Satellites;
using MiraiNavi.Shared.Models.Navi;
using NaviSharp.Time;
using System.Collections.ObjectModel;

namespace MiraiNavi.Shared.Models.Solution;

public record class EpochData
{
    public UtcTime TimeStamp { get; set; }

    public GpsTime GpsTime => GpsTime.FromUtc(TimeStamp);

    public string TimeStampText => TimeStamp.ToString(@"yyyy\/MM\/dd HH:mm:ss.fff", null);

    public NaviResult? Result { get; set; }

    public NaviPrecision? Precision { get; set; }

    public QualityFactors? QualityFactors { get; set; }

    public IEnumerable<SatelliteSkyPosition>? SatelliteSkyPositions { get; set; }

    public IEnumerable<SatelliteTracking>? SatelliteTrackings { get; set; }

}
