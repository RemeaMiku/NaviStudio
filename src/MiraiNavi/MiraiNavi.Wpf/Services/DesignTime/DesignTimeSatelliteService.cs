using System;
using System.Collections.Generic;
using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.Services.DesignTime;

public class DesignTimeSatelliteService : ISatelliteServcie
{
    public IEnumerable<SatelliteTrackingInfo> GetSatelliteTrackingInfos()
    {
        var random = new Random();
        var count = random.Next(15, 30);
        for (int i = 0; i < count; i++)
        {
            yield return new(null, $"G{i + 1:D2}", 360 * (float)random.NextDouble(), 90 * (float)random.NextDouble());
        }
    }
}
