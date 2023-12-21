using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.Shared.Models.Satellites;

public struct SatelliteSignalNoiseRatio
{
    public string Satellite { get; set; }

    public double Frequency { get; set; }

    public double SignalNoiseRatio { get; set; }
}
