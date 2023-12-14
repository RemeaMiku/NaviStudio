using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Common;

public class RealTimeControlOptions(IPEndPoint iPEndPoint)
{
    public static readonly RealTimeControlOptions Default = new(new IPEndPoint(IPAddress.Loopback, 39831));

    public IPEndPoint IPEndPoint { get; set; } = iPEndPoint;

    public int MaxEpochCount { get; set; }

    public UtcTime StartTime { get; set; }

    public TimeSpan Duration { get; set; }
}
