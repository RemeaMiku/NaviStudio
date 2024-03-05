using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviStudio.IPS;

public class IPSSatData
{
    public char[] name = null!;             ///< 卫星名称: G01,R03,C05,E04
	public double[] azel = null!;             ///< azimuth/elevation angles {az,el} (deg)
	public bool[] IsUsed = null!;       ///是否参与解算  NFREQ=3对应三个频点
	public double[] frq = null!;          ///< 频率(HZ), frq = C/lam
	public double[] S = null!;			   ///< signal strength
}
