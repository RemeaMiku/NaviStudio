using System.Collections.Frozen;
using NaviStudio.Shared.Models.Chart;

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

    public static FrozenDictionary<string, Dictionary<string, Func<EpochData, double?>>> ChartItemFuncs { get; }

    #endregion Public Properties

    #region Private Methods

    static FrozenDictionary<string, Dictionary<string, Func<EpochData, double?>>> GetDefaultChartItemFuncs()
    {
        return new Dictionary<string, Dictionary<string, Func<EpochData, double?>>>()
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
        }.ToFrozenDictionary();
    }

    #endregion Private Methods
}
