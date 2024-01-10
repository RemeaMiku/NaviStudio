using System.Net;

namespace MiraiNavi.Shared.Models.RealTime;

public class RealTimeSolutionOptions(string name)
{
    public static readonly IPEndPoint DefaultEpochDataIPEndPoint = new(IPAddress.Loopback, 39831);

    public string Name { get; set; } = name;

    public IPEndPoint BaseRoverEndPoint { get; set; } = null!;

    public IPEndPoint EpochDataIPEndPoint { get; set; } = DefaultEpochDataIPEndPoint;

    public int MaxEpochCount { get; set; } = int.MaxValue;
}
