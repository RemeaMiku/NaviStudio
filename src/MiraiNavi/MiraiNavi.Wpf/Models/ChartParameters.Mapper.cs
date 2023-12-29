using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Models;

partial class ChartParameters
{
    public static ChartParameters FromChartItem(string chartItem) => _chartParas[chartItem];

    readonly static string[] _xyz = ["X", "Y", "Z"];
    readonly static string[] _enu = ["E", "N", "U"];
    readonly static string[] _eulerAngles = ["航向角", "俯仰角", "横滚角"];

    readonly static FrozenDictionary<string, ChartParameters> _chartParas = new Dictionary<string, ChartParameters>()
    {
        {
            ChartItems.LongitudeAndLatitude, new()
            {
                Title = ChartItems.LongitudeAndLatitude,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "纬度", epochData =>  epochData.Pose?.GeodeticCoord.Latitude.Degrees },
                    { "经度", epochData =>  epochData.Pose?.GeodeticCoord.Longitude.Degrees }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.Altitude, new()
            {
                Title = ChartItems.Altitude,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "椭球高", epochData => epochData.Pose?.GeodeticCoord.Altitude }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.LocalPosition, new()
            {
                Title = ChartItems.LocalPosition,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "E", epochData => epochData.EastLocalPosition },
                    { "N", epochData => epochData.NorthLocalPosition },
                    { "U", epochData => epochData.UpLocalPosition }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.Velocity, new()
            {
                Title = ChartItems.Velocity,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "X", epochData => epochData.Pose?.XVelocity },
                    { "Y", epochData => epochData.Pose?.YVelocity },
                    { "Z", epochData => epochData.Pose?.ZVelocity }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.EulerAngles, new()
            {
                Title = ChartItems.EulerAngles,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "航向角", epochData => epochData.Pose?.EulerAngles.Yaw.Degrees },
                    { "俯仰角", epochData => epochData.Pose?.EulerAngles.Pitch.Degrees },
                    { "横滚角", epochData => epochData.Pose?.EulerAngles.Roll.Degrees }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.AccelerometerBias, new()
            {
                Title = ChartItems.AccelerometerBias,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "X", epochData => epochData.ImuBias?.AccelerometerBias.X },
                    { "Y", epochData => epochData.ImuBias?.AccelerometerBias.Y },
                    { "Z", epochData => epochData.ImuBias?.AccelerometerBias.Z }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.GyroscopeBias, new()
            {
                Title = ChartItems.GyroscopeBias,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "X", epochData => epochData.ImuBias?.GyroscopeBias.X },
                    { "Y", epochData => epochData.ImuBias?.GyroscopeBias.Y },
                    { "Z", epochData => epochData.ImuBias?.GyroscopeBias.Z }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.StdLocalPosition, new()
            {
                Title = ChartItems.StdLocalPosition,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "E", epochData => epochData.LocalPositionPrecision?.StdEast},
                    { "N", epochData => epochData.LocalPositionPrecision?.StdNorth},
                    { "U", epochData => epochData.LocalPositionPrecision?.StdUp }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.StdVelocity, new()
            {
                Title = ChartItems.StdVelocity,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "E", epochData => epochData.PosePrecision?.StdEastVelocity },
                    { "N", epochData => epochData.PosePrecision?.StdNorthVelocity },
                    { "U", epochData => epochData.PosePrecision?.StdUpVelocity }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.StdEulerAngles, new()
            {
                Title = ChartItems.StdEulerAngles,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "航向角", epochData => epochData.PosePrecision?.StdYaw.Degrees },
                    { "俯仰角", epochData => epochData.PosePrecision?.StdPitch.Degrees },
                    { "横滚角", epochData => epochData.PosePrecision?.StdRoll.Degrees }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.StdAccelerometerBias, new()
            {
                Title = ChartItems.StdAccelerometerBias,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "X", epochData => epochData.ImuBiasPrecision?.StdAccelerometerBias.X },
                    { "Y", epochData => epochData.ImuBiasPrecision?.StdAccelerometerBias.Y },
                    { "Z", epochData => epochData.ImuBiasPrecision?.StdAccelerometerBias.Z }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.StdGyroscopeBias, new()
            {
                Title = ChartItems.StdGyroscopeBias,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "X", epochData => epochData.ImuBiasPrecision?.StdGyroscopeBias.X },
                    { "Y", epochData => epochData.ImuBiasPrecision?.StdGyroscopeBias.Y },
                    { "Z", epochData => epochData.ImuBiasPrecision?.StdGyroscopeBias.Z }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.Dop, new()
            {
                Title = ChartItems.Dop,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { "PDOP", epochData => epochData.Pdop },
                    { "HDOP", epochData => epochData.Hdop },
                    { "VDOP", epochData => epochData.Vdop },
                    { "GDOP", epochData => epochData.Gdop }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.Ratio, new()
            {
                Title = ChartItems.Ratio,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { ChartItems.Ratio, epochData => epochData.Ratio }
                }.ToFrozenDictionary()
            }
        },
        {
            ChartItems.SatelliteCount,
            new()
            {
                Title= ChartItems.SatelliteCount,
                LabelFuncs=new Dictionary<string, Func<EpochData, double?>>()
                {
                    { ChartItems.SatelliteCount, epochData => epochData.Satellites?.Count }
                }.ToFrozenDictionary()
            }
        }
    }.ToFrozenDictionary();
}
