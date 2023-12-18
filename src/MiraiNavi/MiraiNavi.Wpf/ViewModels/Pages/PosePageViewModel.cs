using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Common.Messages;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class PosePageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableRecipient(messenger), IRecipient<EpochData>, IRecipient<NotificationMessage>
{
    readonly IEpochDatasService _epochDatasService = epochDatasService;

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

    void Reset()
    {
        TimeStamp = default;
        Latitude = double.NaN;
        Longitude = double.NaN;
        Altitude = double.NaN;
        Yaw = double.NaN;
        Pitch = double.NaN;
        Roll = double.NaN;
        Velocity = double.NaN;
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        Sync();
    }

    void Sync()
    {
        if (_epochDatasService.LastestData is not null)
            Receive(_epochDatasService.LastestData);
        else
            Reset();
    }

    public void Receive(EpochData data)
    {
        TimeStamp = data.TimeStamp;
        if (data.Pose is null)
        {
            Reset();
            return;
        }
        Latitude = data.Pose.GeodeticCoord.Latitude.Degrees;
        Longitude = data.Pose.GeodeticCoord.Longitude.Degrees;
        Altitude = data.Pose.GeodeticCoord.Altitude;
        Yaw = data.Pose.EulerAngles.Yaw.Degrees;
        Pitch = data.Pose.EulerAngles.Pitch.Degrees;
        Roll = data.Pose.EulerAngles.Roll.Degrees;
        Velocity = data.Pose.Velocity;
    }

    public void Receive(NotificationMessage message)
    {
        if (message.Type == NotificationType.Reset)
            Reset();
        if (message.Type == NotificationType.Sync)
            Sync();
    }
}
