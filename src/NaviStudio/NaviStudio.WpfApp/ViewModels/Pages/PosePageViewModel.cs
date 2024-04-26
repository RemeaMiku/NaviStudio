using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using NaviSharp.Time;
using NaviStudio.WpfApp.Services.Contracts;
using NaviStudio.WpfApp.ViewModels.Base;

namespace NaviStudio.WpfApp.ViewModels.Pages;

public partial class PosePageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableSingleEpochDataRecipient(messenger, epochDatasService)
{
    #region Public Fields

    public const string Title = "位姿";
    public const string MenuItemHeader = $"{Title}(_N)";

    #endregion Public Fields

    #region Protected Methods

    public override void Reset()
    {
        TimeStamp = _notAvailable;
        GpsTime = _notAvailable;
        Latitude = double.NaN;
        Longitude = double.NaN;
        Altitude = double.NaN;
        Yaw = double.NaN;
        Pitch = double.NaN;
        Roll = double.NaN;
        Velocity = double.NaN;
        EastVelocity = double.NaN;
        NorthVelocity = double.NaN;
        UpVelocity = double.NaN;
    }

    public override void Update(EpochData data)
    {
        TimeStamp = data.DisplayTimeStamp;
        GpsTime = $"{data.GpsTime.Week} 周 {data.GpsTime.Sow:F3} 秒";
        if(data.Result is null)
        {
            Reset();
            return;
        }
        Latitude = data.Result.GeodeticCoord.Latitude.Degrees;
        Longitude = data.Result.GeodeticCoord.Longitude.Degrees;
        Altitude = data.Result.GeodeticCoord.Altitude;
        Yaw = data.Result.Attitude.Yaw.Degrees;
        Pitch = data.Result.Attitude.Pitch.Degrees;
        Roll = data.Result.Attitude.Roll.Degrees;
        Velocity = data.Result.Velocity.Length();
        EastVelocity = data.Result.Velocity.E;
        NorthVelocity = data.Result.Velocity.N;
        UpVelocity = data.Result.Velocity.U;
    }

    #endregion Protected Methods

    const string _notAvailable = "不可用";

    #region Private Fields

    [ObservableProperty]
    string _timeStamp = _notAvailable;

    [ObservableProperty]
    string _gpsTime = _notAvailable;

    [ObservableProperty]
    double _latitude;

    [ObservableProperty]
    double _longitude;

    [ObservableProperty]
    double _altitude;

    [ObservableProperty]
    double _yaw;

    [ObservableProperty]
    double _pitch;

    [ObservableProperty]
    double _roll;

    [ObservableProperty]
    double _velocity;

    [ObservableProperty]
    double _eastVelocity;

    [ObservableProperty]
    double _northVelocity;

    [ObservableProperty]
    double _upVelocity;

    #endregion Private Fields
}
