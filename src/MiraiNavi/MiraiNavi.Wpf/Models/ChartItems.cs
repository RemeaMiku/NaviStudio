using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Models;

public static class ChartItems
{
    public const string LongitudeAndLatitude = "经纬度(°)";

    public const string LocalCoord = "本地坐标(m)";

    public const string Altitude = "椭球高(m)";

    public const string Velocity = "速度(m/s)";

    public const string Attitude = "姿态角(°)";

    public const string AccelerometerBias = "加速度计零偏(m/s²)";

    public const string GyroscopeBias = "陀螺仪零偏(°/s)";

    public const string StdLocalCoord = "本地坐标标准差(m)";

    public const string StdVelocity = "速度标准差(m/s)";

    public const string StdAttitude = "姿态角标准差(°)";

    public const string StdAccelerometerBias = "加速度计零偏标准差(m/s²)";

    public const string StdGyroscopeBias = "陀螺仪零偏标准差(°/s)";

    public const string Dop = "DOP";

    public const string Ratio = "Ratio";

    public const string VisibleSatelliteCount = "可视卫星数量";

    public const string SatelliteVisibility = "卫星可视性";
}
