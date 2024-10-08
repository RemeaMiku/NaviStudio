﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using NaviStudio.Shared.Common.Helpers;
using NaviStudio.WpfApp.Services.Contracts;
using NaviStudio.WpfApp.ViewModels.Base;

namespace NaviStudio.WpfApp.ViewModels.Pages;

public partial class DashBoardPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableSingleEpochDataRecipient(messenger, epochDatasService)
{
    #region Public Fields

    public const string Title = "仪表盘";
    public const string MenuItemHeader = $"{Title}(_D)";

    #endregion Public Fields

    #region Public Properties

    public double PitchTranslateY => 5 * Pitch;

    public double HorizonOffset => Math.Clamp(0.5 + Pitch / 35.7142857142857, 0, 1);

    #endregion Public Properties

    #region Protected Methods

    protected override void OnActivated()
    {
        base.OnActivated();
        Sync();
    }

    public override void Reset()
    {
        Speed = default;
        Yaw = default;
        Pitch = default;
        Roll = default;
    }

    public override void Update(EpochData data)
    {
        if(data.Result is null)
        {
            Reset();
            return;
        }
        Speed = UnitConverter.MetersPerSecondToKilometersPerHour(data.Result.Velocity.Length());
        Yaw = data.Result.Attitude.Yaw.Degrees;
        Pitch = data.Result.Attitude.Pitch.Degrees;
        Roll = data.Result.Attitude.Roll.Degrees;
    }

    #endregion Protected Methods

    #region Private Fields

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

    #endregion Private Fields
}
