using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviStudio.IPS;

public class IPSGnssSolution
{
    //解算结果
    public IPSGpsTime m_time;      //GPS周 周秒
    public double[] m_XYZPos = null!; //EcefCoord
    public double[] m_LLHPos = null!; //GeodeticCoord Deg
    public double[] m_BLENU = null!;  //LocalCoord
    public double[] m_ENUVel = null!; //Velocity ENU
    public double[] m_Att = null!;    //Attitude Deg
    //解算精度
    public double[] m_ENUPosP = null!;//StdLocalCoord
    public double[] m_ENUVelP = null!;//StdVelocity
    public double[] m_AttP = null!;   //StdAttitude
    //质量控制
    public double m_ARRatio;   //AmbFixedRatio
    public double m_HDOP;
    public double m_VDOP;
    public double m_PDOP;
    public double m_GDOP;
    //卫星数据
    public IPSSatData[] m_pSatData = null!;
}
