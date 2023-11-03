using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        var infos = _satelliteServcie.GetSatelliteTrackingInfos();
        foreach (var info in infos)
        {
            switch (info.SatelliteInfo.SystemType)
            {
                case SatelliteSystemType.GPS:
                    GpsInfos.Add(info);
                    break;
                case SatelliteSystemType.BDS:
                    BdsInfos.Add(info);
                    break;
                case SatelliteSystemType.GLONASS:
                    GlonassInfos.Add(info);
                    break;
                case SatelliteSystemType.Galileo:
                    GalileoInfos.Add(info);
                    break;
                case SatelliteSystemType.Others:
                    break;
                default:
                    break;
            }
        }
    }


    public ObservableCollection<SatelliteTrackingInfo>? GpsInfos { get; } = new();


    public ObservableCollection<SatelliteTrackingInfo>? BdsInfos { get; } = new();


    public ObservableCollection<SatelliteTrackingInfo>? GlonassInfos { get; } = new();


    public ObservableCollection<SatelliteTrackingInfo>? GalileoInfos { get; } = new();
}
