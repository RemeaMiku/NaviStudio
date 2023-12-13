using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Common.Helpers;
using MiraiNavi.WpfApp.Models.Navigation;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class DashBoardPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableRecipient(messenger), IRecipient<EpochData>
{
    readonly IEpochDatasService _epochDatasService = epochDatasService;

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
        var newestData = _epochDatasService.Datas.LastOrDefault();
        if (newestData is null)
            Reset();
        else
            Receive(newestData);
    }

    public void Reset()
    {
        Speed = default;
        Yaw = default;
        Pitch = default;
        Roll = default;
    }

    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        Reset();
    }

    public void Receive(EpochData message)
    {
        Speed = UnitConverter.MetersPerSecondToKilometersPerHour(message.NavigationParameters.Velocity);
        Yaw = message.NavigationParameters.EulerAngles.Yaw.Degrees;
        Pitch = message.NavigationParameters.EulerAngles.Pitch.Degrees;
        Roll = message.NavigationParameters.EulerAngles.Roll.Degrees;
    }
}
