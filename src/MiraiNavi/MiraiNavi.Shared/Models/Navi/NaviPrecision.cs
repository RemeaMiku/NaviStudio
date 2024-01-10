using NaviSharp;

namespace MiraiNavi.Shared.Models.Navi;

public class NaviPrecision
{
    public Enu StdLocalCoord { get; set; }

    public Enu StdVelocity { get; set; }

    public EulerAngles StdAttitude { get; set; }
}
