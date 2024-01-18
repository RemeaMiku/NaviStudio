using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MiraiNavi.Shared.Models.Options;

namespace MiraiNavi.WpfApp.Common.Settings;

public class SolutionSettings
{
    public static readonly IPEndPointOptions DefaultEpochDataIPEndPointOptions = new(IPAddress.Loopback.ToString(), 39831);

    public IPEndPointOptions EpochDataEndPointOptions { get; set; } = DefaultEpochDataIPEndPointOptions;
}
