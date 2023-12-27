using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.Shared.Common.Helpers;
using MiraiNavi.WpfApp.Common.Helpers;
using MiraiNavi.WpfApp.Common.Messages;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class DashBoardPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationEpochDataRecipient(messenger, epochDatasService)
{
    public static string Title => "仪表盘";

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
        Sync();
    }

    protected override void Reset()
    {
        Speed = default;
        Yaw = default;
        Pitch = default;
        Roll = default;
    }

    public override void Receive(EpochData data)
    {
        if (data.Pose is null)
        {
            Reset();
            return;
        }
        Speed = UnitConverter.MetersPerSecondToKilometersPerHour(data.Pose.Velocity);
        Yaw = data.Pose.EulerAngles.Yaw.Degrees;
        Pitch = data.Pose.EulerAngles.Pitch.Degrees;
        Roll = data.Pose.EulerAngles.Roll.Degrees;
    }
}
