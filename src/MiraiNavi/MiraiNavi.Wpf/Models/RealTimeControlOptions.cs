﻿using System.Net;

namespace MiraiNavi.WpfApp.Models;

public class RealTimeControlOptions(IPEndPoint iPEndPoint)
{
    public static readonly RealTimeControlOptions Default = new(new IPEndPoint(IPAddress.Loopback, 39831));

    public string Name { get; set; } = "默认配置";

    public IPEndPoint IPEndPoint { get; set; } = iPEndPoint;

    public int MaxEpochCount { get; set; }

    public UtcTime StartTime { get; set; }

    public TimeSpan Duration { get; set; }
}
