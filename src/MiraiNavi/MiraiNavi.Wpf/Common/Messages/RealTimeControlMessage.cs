using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.Common.Messages;

public class RealTimeControlMessage(RealTimeControlMode mode)
{
    public UtcTime UtcTime { get; init; } = UtcTime.Now;

    public RealTimeControlMode Mode { get; init; } = mode;
}
