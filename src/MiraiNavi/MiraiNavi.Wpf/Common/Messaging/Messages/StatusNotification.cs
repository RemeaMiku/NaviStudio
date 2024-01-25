namespace MiraiNavi.WpfApp.Common.Messaging.Messages;

public class StatusNotification(string content, SeverityType type, bool isProcessing = false)
{
    #region Public Properties

    public SeverityType Type { get; set; } = type;

    public bool IsProcessing { get; set; } = isProcessing;

    public string Content { get; set; } = content;

    #endregion Public Properties
}
