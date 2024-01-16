using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Common.Settings;

public class SolutionSettings
{
    public static readonly IPEndPoint DefaultEpochDataIPEndPoint = new(IPAddress.Loopback, 39831);

    public IPEndPoint EpochDataEndPoint { get; set; } = DefaultEpochDataIPEndPoint;
}
