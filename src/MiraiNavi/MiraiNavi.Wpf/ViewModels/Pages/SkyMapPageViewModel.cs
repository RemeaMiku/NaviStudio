using System.Collections.ObjectModel;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class SkyMapPageViewModel(ISatelliteServcie satelliteServcie) : BaseViewModel
{
    readonly ISatelliteServcie _satelliteServcie = satelliteServcie;

    public ObservableCollection<SatelliteTrackingInfo> SatelliteTrackingInfos { get; private set; } =
    [
        new() { Satellite = "G01", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "G02", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "G03", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "G04", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "G05", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "C01", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "C02", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "C03", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "R01", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "R02", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "R03", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "R04", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "R05", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "E01", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "E02", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "E03", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new() { Satellite = "E04", Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
    ];
}
