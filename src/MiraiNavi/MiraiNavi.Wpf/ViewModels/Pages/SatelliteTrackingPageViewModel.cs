using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class SkyMapPageViewModel : BaseViewModel
{
    readonly ISatelliteServcie _satelliteServcie;
    public SkyMapPageViewModel(ISatelliteServcie satelliteServcie)
    {
        _satelliteServcie = satelliteServcie;
        Infos = _satelliteServcie.GetSatelliteTrackingInfos();
    }

    [ObservableProperty]
    IEnumerable<SatelliteTrackingInfo>? _infos;
}
