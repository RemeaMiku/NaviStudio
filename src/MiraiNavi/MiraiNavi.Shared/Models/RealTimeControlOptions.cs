using System.Net;

namespace MiraiNavi.Shared.Models;

public class RealTimeControlOptions(string name)
{
    public static readonly IPEndPoint DefaultIPEndPoint = new(IPAddress.Loopback, 39831);

    public string Name { get; set; } = name;

    public IPEndPoint IPEndPoint { get; set; } = DefaultIPEndPoint;

    public int? MaxEpochCount { get; set; }

    public UtcTime? StartTime { get; set; }

    public TimeSpan? Duration { get; set; }
}
