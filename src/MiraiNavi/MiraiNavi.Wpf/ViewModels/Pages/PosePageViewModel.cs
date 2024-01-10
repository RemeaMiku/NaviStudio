using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MiraiNavi.Shared.Models.Solution;
using MiraiNavi.WpfApp.Common.Messages;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;
using NaviSharp.Time;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class PosePageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationEpochDataRecipient(messenger, epochDatasService)
{
    public const string Title = "位姿";
    public const string MenuItemHeader = $"{Title}(_P)";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(GpsTime))]
    UtcTime _timeStamp;

    public GpsTime GpsTime => GpsTime.FromUtc(TimeStamp);

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
        if (data.Result is null)
        {
            Reset();
            Messenger.Send(new Output(UtcTime.Now, Title, InfoBarSeverity.Warning, $"{data.TimeStamp} 历元位姿数据异常"));
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
