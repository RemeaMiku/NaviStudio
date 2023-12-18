using System.Net;

namespace MiraiNavi.WpfApp.Models;

public class RealTimeControlOptions(string name)
{
    public string Name { get; set; } = name;

    public IPEndPoint IPEndPoint { get; set; } = new(IPAddress.Loopback, 39831);

    public int? MaxEpochCount { get; set; }

    public UtcTime? StartTime { get; set; }

    public TimeSpan? Duration { get; set; }
}
