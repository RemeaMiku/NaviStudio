namespace NaviStudio.Shared.Models;

public class Output(string senderName, SeverityType type, string message, string? details = default)
{
    #region Public Properties

    public UtcTime TimeStamp { get; } = UtcTime.Now;

    public string SenderName { get; init; } = senderName;

    public SeverityType Type { get; init; } = type;

    public string Message { get; init; } = message;

    public string DisplayMessage => $"[{SenderName}] {Message}";

    public string? Details { get; init; } = details;

    #endregion Public Properties
}
