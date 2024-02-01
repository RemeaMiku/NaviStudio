using NaviSharp;

namespace NaviStudio.Shared.Models.Navi;

public class NaviPrecision
{
    #region Public Properties

    public Enu StdLocalCoord { get; set; }

    public Enu StdVelocity { get; set; }

    public EulerAngles StdAttitude { get; set; }

    #endregion Public Properties
}
