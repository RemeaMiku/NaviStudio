﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace MiraiNavi.WpfApp.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool _isBusy;

    bool IsNotBusy => !IsBusy;
}
