using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Models;

public class Output(UtcTime timeStamp, string senderName, InfoBarSeverity severity, string message)
{
    public UtcTime TimeStamp { get; } = timeStamp;

    public string SenderName { get; init; } = senderName;

    public InfoBarSeverity Severity { get; init; } = severity;

    public string Message { get; init; } = message;

    public string DisplayMessage => $"[{SenderName}] {Message}";
}
