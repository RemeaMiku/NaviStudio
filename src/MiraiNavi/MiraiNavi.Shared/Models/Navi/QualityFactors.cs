using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.Shared.Models.Navi;

public record class QualityFactors
{
    public double AmbFixedRatio { get; set; }

    public double HDop { get; set; }

    public double VDop { get; set; }

    public double PDop { get; set; }

    public double GDop { get; set; }

    public int VisibleSalliteCount { get; set; }
}
