using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class PosePageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationRecipient(messenger, epochDatasService)
{
    public const string Title = "位姿";
    public const string MenuItemHeader = $"{Title}(_P)";

    [ObservableProperty]
    UtcTime _timeStamp;

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

    protected override void Reset()
    {
        TimeStamp = default;
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

    protected override void Update(EpochData data)
    {
        TimeStamp = data.TimeStamp;
        if (data.Result is null)
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
}
