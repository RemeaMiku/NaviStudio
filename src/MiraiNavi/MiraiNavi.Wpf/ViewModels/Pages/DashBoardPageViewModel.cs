using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MiraiNavi.WpfApp.Common.Helpers;
using MiraiNavi.WpfApp.Common.Messages;
using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class DashBoardPageViewModel(IMessenger messenger) : ObservableRecipient(messenger), IRecipient<EpochData>, IRecipient<RealTimeControlMessage>
{
    [ObservableProperty]
    double _speed;

    [ObservableProperty]
    double _yaw;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HorizonOffset))]
    [NotifyPropertyChangedFor(nameof(PitchTranslateY))]
    double _pitch;

    [ObservableProperty]
    double _roll;

    public double PitchTranslateY => 5 * Pitch;

    public double HorizonOffset => Math.Clamp(0.5 + Pitch / 35.7142857142857, 0, 1);

    protected override void OnActivated()
    {
        base.OnActivated();
        var message = Messenger.Send(new RequestMessage<EpochData>());
        if (message.HasReceivedResponse)
            Receive(message);
        else
            Reset();
    }

    public void Reset()
    {
        Speed = default;
        Yaw = default;
        Pitch = default;
        Roll = default;
    }

    public void Receive(EpochData message)
    {
        ArgumentNullException.ThrowIfNull(message.Pose);
        Speed = UnitConverter.MetersPerSecondToKilometersPerHour(message.Pose.Velocity);
        Yaw = message.Pose.EulerAngles.Yaw.Degrees;
        Pitch = message.Pose.EulerAngles.Pitch.Degrees;
        Roll = message.Pose.EulerAngles.Roll.Degrees;
    }

    public void Receive(RealTimeControlMessage message)
    {
        if (message.Mode == RealTimeControlMode.Start)
            Reset();
    }
}
