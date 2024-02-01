using NaviSharp;
using NaviSharp.SpatialReference;

namespace NaviStudio.Shared.Models.Navi;

public class NaviResult
{
    #region Public Properties

    public Xyz EcefCoord { get; set; }

    public GeodeticCoord GeodeticCoord { get; set; }

    public Enu Velocity { get; set; }

    public Enu LocalCoord { get; set; }

    public EulerAngles Attitude { get; set; }

    #endregion Public Properties

}
