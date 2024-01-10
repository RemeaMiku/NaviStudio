using System.ComponentModel.DataAnnotations;
using NaviSharp;

namespace MiraiNavi.Shared.Models.Navi;

public record class NaviPrecision
{
    public Enu StdLocalCoord { get; set; }

    public Enu StdVelocity { get; set; }

    public EulerAngles StdAttitude { get; set; }
}
