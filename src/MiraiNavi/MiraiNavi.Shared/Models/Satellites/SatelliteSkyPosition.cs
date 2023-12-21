using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.Shared.Models.Satellites;

public struct SatelliteSkyPosition
{
    double _azimuth;
    double _elevation;

    public Satellite Satellite { get; set; }

    /// <summary>
    /// 方位角
    /// </summary>
    public double Azimuth
    {
        readonly get => _azimuth;
        set => _azimuth = Math.Clamp(value, 0, 360);
    }

    /// <summary>
    /// 高度角
    /// </summary>
    public double Elevation
    {
        readonly get => _elevation;
        set => _elevation = Math.Clamp(value, 0, 90);
    }
}
