using System.Collections.Frozen;
using MiraiNavi.Shared.Models.Solution;

namespace MiraiNavi.WpfApp.Models;

partial class ChartParameters
{
    public static ChartParameters? FromChartItem(string chartItem) => _chartParas.GetValueOrDefault(chartItem);

    //readonly static string[] _xyz = ["X", "Y", "Z"];
    //readonly static string[] _enu = ["E", "N", "U"];
    //readonly static string[] _eulerAngles = ["航向角", "俯仰角", "横滚角"];

    readonly static FrozenDictionary<string, ChartParameters> _chartParas = new Dictionary<string, ChartParameters>()
    {
        {
            ChartItems.LongitudeAndLatitude, new()
            {
                Title = ChartItems.LongitudeAndLatitude,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "纬度", epochData =>  epochData.Result?.GeodeticCoord.Latitude.Degrees },
                    { "经度", epochData =>  epochData.Result?.GeodeticCoord.Longitude.Degrees }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.Altitude, new()
            {
                Title = ChartItems.Altitude,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "椭球高", epochData => epochData.Result?.GeodeticCoord.Altitude }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.LocalCoord, new()
            {
                Title = ChartItems.LocalCoord,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "E", epochData => epochData.Result?.LocalCoord.E },
                    { "N", epochData => epochData.Result?.LocalCoord.N },
                    { "U", epochData => epochData.Result?.LocalCoord.U },
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.Velocity, new()
            {
                Title = ChartItems.Velocity,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "X", epochData => epochData.Result?.Velocity.E },
                    { "Y", epochData => epochData.Result?.Velocity.N },
                    { "Z", epochData => epochData.Result?.Velocity.U }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.Attitude, new()
            {
                Title = ChartItems.Attitude,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "航向角", epochData => epochData.Result?.Attitude.Yaw.Degrees },
                    { "俯仰角", epochData => epochData.Result?.Attitude.Pitch.Degrees },
                    { "横滚角", epochData => epochData.Result?.Attitude.Roll.Degrees }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.StdLocalCoord, new()
            {
                Title = ChartItems.StdLocalCoord,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "E", epochData => epochData.Precision?.StdLocalCoord.E },
                    { "N", epochData => epochData.Precision ?.StdLocalCoord.N },
                    { "U", epochData => epochData.Precision ?.StdLocalCoord.U }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.StdVelocity, new()
            {
                Title = ChartItems.StdVelocity,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "E", epochData => epochData.Precision?.StdVelocity.E },
                    { "N", epochData => epochData.Precision ?.StdVelocity.N } ,
                    { "U", epochData => epochData.Precision ?.StdVelocity.U }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.StdAttitude, new()
            {
                Title = ChartItems.StdAttitude,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "航向角", epochData => epochData.Precision?.StdAttitude.Yaw.Degrees },
                    { "俯仰角", epochData => epochData.Precision?.StdAttitude.Pitch.Degrees },
                    { "横滚角", epochData => epochData.Precision?.StdAttitude.Roll.Degrees }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.Dop, new()
            {
                Title = ChartItems.Dop,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "PDOP", epochData => epochData.QualityFactors?.PDop },
                    { "HDOP", epochData => epochData.QualityFactors ?.HDop },
                    { "VDOP", epochData => epochData.QualityFactors ?.VDop },
                    { "GDOP", epochData => epochData.QualityFactors ?.GDop }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.Ratio, new()
            {
                Title = ChartItems.Ratio,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { ChartItems.Ratio, epochData => epochData.QualityFactors?.AmbFixedRatio }
                }.ToFrozenDictionary()
            }
        },
    }.ToFrozenDictionary();
}
