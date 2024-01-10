using MiraiNavi.Shared.Models.Satellites;
using MiraiNavi.Shared.Models.Navi;
using NaviSharp.Time;
using System.Collections.ObjectModel;

namespace MiraiNavi.Shared.Models;

public class EpochData
{
    public UtcTime TimeStamp { get; set; }

    public string DisplayTimeStamp => TimeStamp.ToString(@"yyyy\/MM\/dd HH:mm:ss.fff", null);

    public GpsTime GpsTime => GpsTime.FromUtc(TimeStamp);

    public NaviResult? Result { get; set; }

    public NaviPrecision? Precision { get; set; }

    public QualityFactors? QualityFactors { get; set; }

    public List<SatelliteSkyPosition>? SatelliteSkyPositions { get; set; }

    public List<SatelliteTracking>? SatelliteTrackings { get; set; }

}
