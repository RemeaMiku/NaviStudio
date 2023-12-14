using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MiraiNavi.WpfApp.Common.Messages;

public class GlobalControlMessage(GlobalControlMessage.ControlMode mode)
{
    public UtcTime UtcTime { get; init; } = UtcTime.Now;

    public ControlMode Mode { get; init; } = mode;

    public enum ControlMode
    {
        Start,
        Pause,
        Stop
    }
}
