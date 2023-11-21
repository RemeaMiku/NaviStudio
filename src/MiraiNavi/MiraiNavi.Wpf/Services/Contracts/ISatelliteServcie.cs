using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface ISatelliteServcie
{
    public IEnumerable<SatelliteTrackingInfo> GetSatelliteTrackingInfos();
}
