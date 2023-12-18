using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.Common.Messages;

public class NotificationMessage(NotificationType type)
{
    public UtcTime TimeStamp { get; init; } = UtcTime.Now;

    public NotificationType Type { get; init; } = type;
}
