using System.Collections.ObjectModel;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class SkyMapPageViewModel(ISatelliteServcie satelliteServcie) : BaseViewModel
{
    readonly ISatelliteServcie _satelliteServcie = satelliteServcie;

    public ObservableCollection<SatelliteTrackingInfo> SatelliteTrackingInfos { get; private set; } =
    [
        new("G01") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("G02") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("G03") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("G04") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("G05") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("C01") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("C02") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("C03") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("R01") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("R02") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("R03") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("R04") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("R05") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("E01") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("E02") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("E03") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
        new("E04") { Azimuth = Random.Shared.Next(0, 360), Elevation = Random.Shared.Next(0, 90) },
    ];
}
