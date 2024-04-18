using System.Collections.Frozen;
using System.Linq;
using NaviStudio.Shared.Models.Chart;
using NaviStudio.Shared.Models.Satellites;

namespace NaviStudio.WpfApp.Common.Helpers;

public static class ChartItemManager
{
    #region Public Constructors

    static ChartItemManager()
    {
        ChartItemFuncs = GetDefaultChartItemFuncs();
    }

    #endregion Public Constructors

    #region Public Properties

    public static IDictionary<string, Dictionary<string, Func<EpochData, double?>>> ChartItemFuncs { get; }

    #endregion Public Properties

    #region Private Methods

    private static Dictionary<string, Dictionary<string, Func<EpochData, double?>>> GetDefaultChartItemFuncs()
    {
        var map = new Dictionary<string, Dictionary<string, Func<EpochData, double?>>>()
        {
            {
                ChartItems.LongitudeAndLatitude, new()
                {
                    { "纬度", epochData =>  epochData.Result?.GeodeticCoord.Latitude.Degrees },
                    { "经度", epochData =>  epochData.Result?.GeodeticCoord.Longitude.Degrees }
                }
            },
            {
                ChartItems.Altitude, new()
                {
                    { "椭球高", epochData => epochData.Result?.GeodeticCoord.Altitude }
                }
            },
            {
                ChartItems.LocalCoord, new()
                {
                    { "E", epochData => epochData.Result?.LocalCoord.E },
                    { "N", epochData => epochData.Result?.LocalCoord.N },
                    { "U", epochData => epochData.Result?.LocalCoord.U },
                }
            },
            {
                ChartItems.Velocity, new()
                {
                    { "E", epochData => epochData.Result?.Velocity.E },
                    { "N", epochData => epochData.Result?.Velocity.N },
                    { "U", epochData => epochData.Result?.Velocity.U }
                }
            },
            {
                ChartItems.Attitude, new()
                {
                    { "航向角", epochData => epochData.Result?.Attitude.Yaw.Degrees },
                    { "俯仰角", epochData => epochData.Result?.Attitude.Pitch.Degrees },
                    { "横滚角", epochData => epochData.Result?.Attitude.Roll.Degrees }
                }
            },
            {
                ChartItems.StdLocalCoord, new()
                {
                    { "E", epochData => epochData.Precision?.StdLocalCoord.E },
                    { "N", epochData => epochData.Precision ?.StdLocalCoord.N },
                    { "U", epochData => epochData.Precision ?.StdLocalCoord.U }
                }
            },
            {
                ChartItems.StdVelocity, new()
                {
                    { "E", epochData => epochData.Precision ?.StdVelocity.E },
                    { "N", epochData => epochData.Precision ?.StdVelocity.N },
                    { "U", epochData => epochData.Precision ?.StdVelocity.U } }
            },
            {
                ChartItems.StdAttitude, new()
                {
                    { "航向角", epochData => epochData.Precision ?.StdAttitude.Yaw.Degrees },
                    { "俯仰角", epochData => epochData.Precision ?.StdAttitude.Pitch.Degrees },
                    { "横滚角", epochData => epochData.Precision ?.StdAttitude.Roll.Degrees } }
            },
            {
                ChartItems.Dop, new()
                {
                    { "PDOP", epochData => epochData.QualityFactors?.PDop },
                    { "HDOP", epochData => epochData.QualityFactors ?.HDop },
                    { "VDOP", epochData => epochData.QualityFactors ?.VDop },
                    { "GDOP", epochData => epochData.QualityFactors ?.GDop }
                }
            },
            {
                ChartItems.Ratio, new()
                {
                    { ChartItems.Ratio, epochData => epochData.QualityFactors?.AmbFixedRatio }
                }
            },
            {
                ChartItems.SatelliteCount,new()
                {
                    { ChartItems.SatelliteCount, epochData => epochData.SatelliteSkyPositions?.Count }
                }
            },
            {
                ChartItems.SatelliteCountOfEachSystem,new()
                {
                    { "GPS", epochData=>epochData.SatelliteSkyPositions?.Count(p=>p.Satellite.System==SatelliteSystems.GPS)},
                    { "BDS", epochData=>epochData.SatelliteSkyPositions?.Count(p=>p.Satellite.System==SatelliteSystems.Beidou)},
                    { "GLONASS", epochData=>epochData.SatelliteSkyPositions?.Count(p=>p.Satellite.System==SatelliteSystems.GLONASS)},
                    { "Galileo", epochData=>epochData.SatelliteSkyPositions?.Count(p=>p.Satellite.System==SatelliteSystems.Galileo)},
                    { "其他", epochData=>epochData.SatelliteSkyPositions?.Count(p=>p.Satellite.System == SatelliteSystems.Others)},
                }
            },
        };
        //var gpsSatMap = new Dictionary<string, Func<EpochData, double?>>();
        //for(var i = 1; i <= 32; i++)
        //{
        //    var sat = new Satellite($"G{i:00}");
        //    gpsSatMap.Add(sat.PrnCode, epochData => epochData.SatelliteSkyPositions?.Exists(p => p.Satellite == sat) == true ? sat.Number : 0);
        //}
        //map.Add(ChartItems.GPSSatelliteVisibility, gpsSatMap);
        //var bdsSatMap = new Dictionary<string, Func<EpochData, double?>>();
        //for(var i = 1; i <= 64; i++)
        //{
        //    var sat = new Satellite($"C{i:00}");
        //    bdsSatMap.Add(sat.PrnCode, epochData => epochData.SatelliteSkyPositions?.Exists(p => p.Satellite == sat) == true ? sat.Number : 0);
        //}
        //map.Add(ChartItems.BeidouSatelliteVisibility, bdsSatMap);
        //var glonassSatMap = new Dictionary<string, Func<EpochData, double?>>();
        //for(var i = 1; i <= 24; i++)
        //{
        //    var sat = new Satellite($"R{i:00}");
        //    glonassSatMap.Add(sat.PrnCode, epochData => epochData.SatelliteSkyPositions?.Exists(p => p.Satellite == sat) == true ? sat.Number : 0);
        //}
        //map.Add(ChartItems.GLONASSSatelliteVisibility, glonassSatMap);
        //var galileoSatMap = new Dictionary<string, Func<EpochData, double?>>();
        //for(var i = 1; i <= 30; i++)
        //{
        //    var sat = new Satellite($"E{i:00}");
        //    galileoSatMap.Add(sat.PrnCode, epochData => epochData.SatelliteSkyPositions?.Exists(p => p.Satellite == sat) == true ? sat.Number : 0);
        //}
        //map.Add(ChartItems.GalileoSatelliteVisibility, galileoSatMap);
        return map;
    }

    #endregion Private Methods
}
