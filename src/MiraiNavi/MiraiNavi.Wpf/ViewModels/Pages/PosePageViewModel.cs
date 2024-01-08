using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MiraiNavi.WpfApp.Common.Messages;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class PosePageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationEpochDataRecipient(messenger, epochDatasService)
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
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        Sync();
    }

    protected override void Sync()
    {
        var message = new RequestMessage<EpochData>();
        Messenger.Send(message);
        if (message.HasReceivedResponse)
            Receive(message);
        else
            Reset();
    }

    public override void Receive(EpochData data)
    {
        TimeStamp = data.TimeStamp;
        if (data.Pose is null)
        {
            Reset();
            Messenger.Send(new Output(UtcTime.Now, Title, InfoBarSeverity.Warning, $"{data.TimeStamp} 历元位姿数据异常"));
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
}
