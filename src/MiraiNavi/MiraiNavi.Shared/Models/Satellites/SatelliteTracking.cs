using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.Shared.Models.Satellites;

public record struct SatelliteTracking
{
    public Satellite Satellite { get; set; }

    public bool IsUsed { get; set; }

    public double Frequency { get; set; }

    public double SignalNoiseRatio { get; set; }
}
