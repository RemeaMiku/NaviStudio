using MiraiNavi.WpfApp.Models.Satellite;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.Services.Test;

public class DesignTimeSatelliteService : ISatelliteServcie
{
    public IEnumerable<SatelliteTrackingInfo> GetSatelliteTrackingInfos()
    {
        var random = new Random();
        var count = random.Next(1, 10);
        for (int i = 0; i < count; i++)
        {
            yield return new($"G{i + 1:D2}") { Azimuth = 360 * (float)random.NextDouble(), Elevation = 90 * (float)random.NextDouble() };
            yield return new($"C{i + 1:D2}") { Azimuth = 360 * (float)random.NextDouble(), Elevation = 90 * (float)random.NextDouble() };
            yield return new($"R{i + 1:D2}") { Azimuth = 360 * (float)random.NextDouble(), Elevation = 90 * (float)random.NextDouble() };
            yield return new($"E{i + 1:D2}") { Azimuth = 360 * (float)random.NextDouble(), Elevation = 90 * (float)random.NextDouble() };
        }
    }
}
