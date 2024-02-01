using NaviStudio.Shared.Models.Satellites;
using NaviStudio.Shared.Models.Navi;
using NaviSharp.Time;

namespace NaviStudio.Shared.Models;

public class EpochData
{
    #region Public Properties

    public UtcTime TimeStamp { get; set; }

    public string DisplayTimeStamp => TimeStamp.ToString(@"yyyy\/MM\/dd HH:mm:ss.fff", null);

    public GpsTime GpsTime => GpsTime.FromUtc(TimeStamp);

    public NaviResult? Result { get; set; }

    public NaviPrecision? Precision { get; set; }

    public QualityFactors? QualityFactors { get; set; }

    public List<SatelliteSkyPosition>? SatelliteSkyPositions { get; set; }

    public List<SatelliteTracking>? SatelliteTrackings { get; set; }

    #endregion Public Properties
}
