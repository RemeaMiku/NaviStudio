namespace NaviStudio.Shared.Models.Map;

public readonly struct MapPoint
{
    public UtcTime TimeStamp { get; init; }

    public double Latitude { get; init; }

    public double Longitude { get; init; }

    public double Bearing { get; init; }

    public static MapPoint FromEpochData(EpochData data)
        => new()
        {
            TimeStamp = data.TimeStamp,
            Latitude = data.Result.GeodeticCoord.Latitude.Degrees,
            Longitude = data.Result.GeodeticCoord.Longitude.Degrees,
            Bearing = data.Result.Attitude.Yaw.Degrees
        };
}
