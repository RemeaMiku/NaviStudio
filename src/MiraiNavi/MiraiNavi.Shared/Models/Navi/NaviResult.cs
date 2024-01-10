using NaviSharp;
using NaviSharp.SpatialReference;

namespace MiraiNavi.Shared.Models.Navi;

public class NaviResult
{
    public Xyz EcefCoord { get; set; }

    public GeodeticCoord GeodeticCoord { get; set; }

    public Enu Velocity { get; set; }

    public Enu LocalCoord { get; set; }

    public EulerAngles Attitude { get; set; }

}
