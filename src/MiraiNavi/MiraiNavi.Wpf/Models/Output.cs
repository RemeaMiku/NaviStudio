namespace MiraiNavi.WpfApp.Models;

public class Output(string senderName, OutputType type, string message, string? details = default)
{
    public UtcTime TimeStamp { get; } = UtcTime.Now;

    public string SenderName { get; init; } = senderName;

    public OutputType Type { get; init; } = type;

    public string Message { get; init; } = message;

    public string DisplayMessage => $"[{SenderName}] {Message}";

    public string? Details { get; init; } = details;
}
