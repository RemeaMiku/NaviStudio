using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Models;

public class Output(string senderName, InfoBarSeverity severity, string message, string? details = default)
{
    public UtcTime TimeStamp { get; } = UtcTime.Now;

    public string SenderName { get; init; } = senderName;

    public InfoBarSeverity Severity { get; init; } = severity;

    public string Message { get; init; } = message;

    public string DisplayMessage => $"[{SenderName}] {Message}";

    public string? Details { get; init; } = details;
}
