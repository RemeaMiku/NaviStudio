using GMap.NET;
using MiraiNavi.Shared.Models.Satellites;

namespace MiraiNavi.WpfApp.Common.Helpers;

public static class RandomDataGenerator
{
    public static IEnumerable<PointLatLng> GetPointLatLngs(int count)
    {
        var random = new Random();
        for (var i = 0; i < count; i++)
        {
            var lat = random.NextDouble() * 180 - 90;
            var lng = random.NextDouble() * 360 - 180;
            yield return new PointLatLng(lat, lng);
        }
    }

    public static IEnumerable<Satellite> GetSatellites(int count)
    {
        var random = new Random();
        var systems = Enum.GetValues<SatelliteSystems>();
        for (int i = 0; i < count; i++)
        {
            var system = systems[random.Next(0, systems.Length)];
            var number = random.Next(1, 33);
            yield return $"{(char)system}{number:00}";
        }
    }

    public static SatelliteSkyPosition GetSatelliteSkyPosition(Satellite satellite)
    {
        var random = new Random();
        return new SatelliteSkyPosition
        {
            Satellite = satellite,
            Azimuth = random.NextDouble() * 360,
            Elevation = random.NextDouble() * 90
        };
    }

    public static SatelliteTracking GetSatelliteSignalNoiseRatio(Satellite satellite)
    {
        var random = new Random();
        return new SatelliteTracking
        {
            Satellite = satellite,
            Frequency = 1000 + random.NextDouble() * 1000,
            SignalNoiseRatio = random.NextDouble() * 100
        };
    }
}
