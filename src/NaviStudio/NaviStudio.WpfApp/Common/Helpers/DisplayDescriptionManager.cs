using System.Collections.Frozen;
using NaviStudio.Shared.Models.Navi;
using NaviStudio.Shared.Models.Satellites;
using NaviSharp;
using NaviSharp.SpatialReference;
using NaviSharp.Time;

namespace NaviStudio.WpfApp.Common.Helpers;

public static class DisplayDescriptionManager
{
    #region Public Constructors

    static DisplayDescriptionManager()
    {
        Descriptions = GetDefaultDescriptions();
    }

    #endregion Public Constructors

    #region Public Properties

    public static FrozenDictionary<string, DisplayDescription> Descriptions { get; }

    #endregion Public Properties

    #region Private Methods

    static FrozenDictionary<string, DisplayDescription> GetDefaultDescriptions()
    {
        return new Dictionary<string, DisplayDescription>()
        {
            {
                nameof(EpochData.Result),
                new()
                {
                    DisplayName = "解算结果",
                }
            },
            {
                nameof(EpochData.Precision),
                new()
                {
                    DisplayName = "解算精度",
                }
            },
            {
                nameof(EpochData.QualityFactors),
                new()
                {
                    DisplayName = "质量因子",
                }
            },
            {
                nameof(EpochData.DisplayTimeStamp),
                new()
                {
                    DisplayName = "UTC",
                }
            },
            {
                nameof(EpochData.GpsTime),
                new()
                {
                    DisplayName = "GPS时间",
                }
            },
            {
                nameof(EpochData.SatelliteSkyPositions),
                new()
                {
                    DisplayName = "卫星天空坐标数据",
                }
            },
            {
                nameof(EpochData.SatelliteTrackings),
                new()
                {
                    DisplayName = "卫星信号跟踪数据",
                }
            },
            {
                nameof(Xyz.X),
                new()
                {
                    DisplayName = "X",
                }
            },
            {
                nameof(Xyz.Y),
                new()
                {
                    DisplayName = "Y",
                }
            },
            {
                nameof(Xyz.Z),
                new()
                {
                    DisplayName = "Z",
                }
            },
            {
                nameof(Enu.E),
                new()
                {
                    DisplayName = "E",
                    Description = "东向"
                }
            },
            {
                nameof(Enu.N),
                new()
                {
                    DisplayName = "N",
                    Description = "北向"
                }
            },
            {
                nameof(Enu.U),
                new()
                {
                    DisplayName = "U",
                    Description = "天向"
                }
            },
            {
                nameof(GpsTime.Week),
                new()
                {
                    DisplayName = "周数",
                }
            },
            {
                nameof(GpsTime.Sow),
                new()
                {
                    DisplayName = "周内秒",
                }
            },
            {
                nameof(NaviResult.EcefCoord),
                new()
                {
                    DisplayName = "ECEF 坐标(m)",
                }
            },
            {
                nameof(NaviResult.LocalCoord),
                new()
                {
                    DisplayName = "本地坐标(m)",
                }
            },
            {
                nameof(NaviResult.Attitude),
                new()
                {
                    DisplayName = "姿态角(°)",
                    Description = "航向角：0~360°, 俯仰角：-90°~90°, 横滚角：-180°~180°",
                }
            },
            {
                nameof(NaviResult.Velocity),
                new()
                {
                    DisplayName = "速度(m/s)",
                }
            },
            {
                nameof(NaviResult.GeodeticCoord),
                new()
                {
                    DisplayName = "大地坐标(°,°,m)",
                    Description = "纬度：-90°~90° 负数为南纬, 经度：-180°~180° 负数为西经",
                }
            },
            {
                nameof(NaviPrecision.StdLocalCoord),
                new()
                {
                    DisplayName = "本地坐标标准差(m)",
                }
            },
            {
                nameof(NaviPrecision.StdVelocity),
                new()
                {
                    DisplayName = "速度标准差(m/s)",
                }
            },
            {
                nameof(NaviPrecision.StdAttitude),
                new()
                {
                    DisplayName = "姿态角标准差(°)",
                }
            },
            {
                nameof(QualityFactors.PDop),
                new()
                {
                    DisplayName = "PDOP",
                    Description = "位置精度因子",
                }
            },
            {
                nameof(QualityFactors.HDop),
                new()
                {
                    DisplayName = "HDOP",
                    Description = "水平精度因子",
                }
            },
            {
                nameof(QualityFactors.VDop),
                new()
                {
                    DisplayName = "VDOP",
                    Description = "垂直精度因子",
                }
            },
            {
                nameof(QualityFactors.GDop),
                new()
                {
                    DisplayName = "GDOP",
                    Description = "几何精度因子",
                }
            },
            {
                nameof(QualityFactors.AmbFixedRatio),
                new()
                {
                    DisplayName = "Ratio",
                    Description = "模糊度固定率",
                }
            },
            {
                nameof(Angle.Degrees),
                new()
                {
                    DisplayName = "角度(°)",
                }
            },
            {
                nameof(Angle.Radians),
                new()
                {
                    DisplayName = "弧度",
                }
            },
            {
                nameof(Angle.DegreesMinutesSeconds),
                new()
                {
                    DisplayName = "度分秒(°,′,″)",
                }
            },
            {
                nameof(GeodeticCoord.Latitude),
                new()
                {
                    DisplayName = "纬度(°)",
                }
            },
            {
                nameof(GeodeticCoord.Longitude),
                new()
                {
                    DisplayName = "经度(°)",
                }
            },
            {
                nameof(GeodeticCoord.Altitude),
                new()
                {
                    DisplayName = "椭球高(m)",
                }
            },
            {
                nameof(EulerAngles.Yaw),
                new()
                {
                    DisplayName = "航向角(°)",
                }
            },
            {
                nameof(EulerAngles.Pitch),
                new()
                {
                    DisplayName = "俯仰角(°)",
                }
            },
            {
                nameof(EulerAngles.Roll),
                new()
                {
                    DisplayName = "横滚角(°)",
                }
            },
            {
                nameof(SatelliteSkyPosition.Satellite),
                new()
                {
                    DisplayName = "卫星"
                }
            },
            {
                nameof(SatelliteSkyPosition.Elevation),
                new()
                {
                    DisplayName = "仰角(°)"
                }
            },
            {
                nameof(SatelliteSkyPosition.Azimuth),
                new()
                {
                    DisplayName = "方位角(°)"
                }
            },
            {
                nameof(SatelliteTracking.Frequency),
                new()
                {
                    DisplayName = "频率(Hz)",
                }
            },
            {
                nameof(SatelliteTracking.IsUsed),
                new()
                {
                    DisplayName = "是否使用"
                }
            },
            {
                nameof(SatelliteTracking.SignalNoiseRatio),
                new()
                {
                    DisplayName = "信噪比(dB)",
                }
            },
            {
                nameof(Satellite.System),
                new()
                {
                    DisplayName = "系统",
                    Description = "G: GPS, C: BDS, R: GLONASS, E: Galileo"
                }
            },
            {
                nameof(Satellite.PrnCode),
                new()
                {
                    DisplayName = "PRN码",
                    Description = "GPS: 1~32, BDS: 1~37, GLONASS: 1~24, Galileo: 1~36"
                }
            }
        }.ToFrozenDictionary();
    }

    #endregion Private Methods
}
