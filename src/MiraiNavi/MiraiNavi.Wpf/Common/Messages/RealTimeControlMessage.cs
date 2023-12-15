using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.Common.Messages;

public class RealTimeControlMessage(RealTimeControlMode mode)
{
    public UtcTime UtcTime { get; init; } = UtcTime.Now;

    public RealTimeControlMode Mode { get; init; } = mode;
}
