using System.Net;

namespace MiraiNavi.Shared.Models.Solution.RealTime;

public class RealTimeSolutionOptions(string name)
{
    //public static readonly IPEndPoint DefaultBaseIPEndPoint=new(new(),);

    public static readonly IPEndPoint DefaultRoverIPEndPoint = new(IPAddress.Loopback, 39831);

    public string Name { get; set; } = name;

    public IPEndPoint BaseRoverEndPoint { get; set; } = null!;

    public IPEndPoint RoverIPEndPoint { get; set; } = DefaultRoverIPEndPoint;

    public int MaxEpochCount { get; set; } = int.MaxValue;

    public UtcTime StartTime { get; set; } = UtcTime.MinValue;

    public TimeSpan Duration { get; set; } = TimeSpan.MaxValue;

    public async Task WaitToStartAsync(CancellationToken token)
    {
        var now = UtcTime.Now;
        var delay = StartTime - now;
        if (delay > TimeSpan.Zero)
            await Task.Delay(delay, token);
        else
            StartTime = now;
    }

    public bool NeedStop(int epochCount)
        => epochCount >= MaxEpochCount || UtcTime.Now - StartTime > Duration;
}
