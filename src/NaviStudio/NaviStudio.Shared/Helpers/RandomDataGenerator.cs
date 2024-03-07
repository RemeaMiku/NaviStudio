using NaviStudio.Shared.Models.Satellites;
using NaviSharp;

namespace NaviStudio.Shared.Common.Helpers;

public static class RandomDataGenerator
{
    #region Public Methods

    public static IEnumerable<Satellite> GetSatellites(int count)
    {
        var random = new Random();
        var systems = Enum.GetValues<SatelliteSystems>();
        for(int i = 0; i < count; i++)
        {
            var system = (char)systems[random.Next(0, systems.Length)];
            var number = random.Next(1, 33);
            yield return $"{system}{number:00}";
        }
    }

    public static SatelliteSkyPosition GetSatelliteSkyPosition(Satellite satellite)
    {
        var random = new Random();
        return new SatelliteSkyPosition
        {
            Satellite = satellite,
            Azimuth = Angle.FromDegrees(random.NextDouble() * 360),
            Elevation = Angle.FromDegrees(random.NextDouble() * 90)
        };
    }

    public static IEnumerable<SatelliteSkyPosition> GetSatelliteSkyPositions(IEnumerable<Satellite> satellites)
    {
        foreach(var satellite in satellites)
            yield return GetSatelliteSkyPosition(satellite);
    }

    public static SatelliteTracking GetSatelliteTracking(Satellite satellite)
    {
        var random = new Random();
        return new SatelliteTracking
        {
            Satellite = satellite,
            IsUsed = random.NextDouble() < 0.9,
            Frequency = Math.Round(1000 + random.NextDouble() * 1000, 1),
            SignalNoiseRatio = random.NextDouble() * 100
        };
    }

    public static IEnumerable<SatelliteTracking> GetSatelliteTrackings(IEnumerable<Satellite> satellites)
    {
        foreach(var satellite in satellites)
        {
            yield return GetSatelliteTracking(satellite);
            yield return GetSatelliteTracking(satellite);
        }
    }

    #endregion Public Methods
}
