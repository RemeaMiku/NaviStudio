using MiraiNavi.WpfApp.Models.Navigation;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.Services.DesignTime;

public class DesignTimeSatelliteService : ISatelliteServcie
{
    public IEnumerable<SatelliteTrackingInfo> GetSatelliteTrackingInfos()
    {
        var random = new Random();
        var count = random.Next(1, 10);
        for (int i = 0; i < count; i++)
        {
            yield return new(null, $"G{i + 1:D2}", 360 * (float)random.NextDouble(), 90 * (float)random.NextDouble());
            yield return new(null, $"C{i + 1:D2}", 360 * (float)random.NextDouble(), 90 * (float)random.NextDouble());
            yield return new(null, $"R{i + 1:D2}", 360 * (float)random.NextDouble(), 90 * (float)random.NextDouble());
            yield return new(null, $"E{i + 1:D2}", 360 * (float)random.NextDouble(), 90 * (float)random.NextDouble());
        }
    }
}
