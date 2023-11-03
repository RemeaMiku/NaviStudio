using System.Collections.Generic;
using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.Services;

public interface ISatelliteServcie
{
    public IEnumerable<SatelliteTrackingInfo> GetSatelliteTrackingInfos();
}
