namespace NaviStudio.WpfApp.Common.Messaging.Messages;

public class NotificationMessage
{
    #region Public Fields

    public static readonly NotificationMessage Reset = new();
    public static readonly NotificationMessage Update = new();
    public static readonly NotificationMessage Sync = new();
    public static readonly NotificationMessage Stop = new();

    #endregion Public Fields

    #region Private Constructors

    private NotificationMessage() { }

    #endregion Private Constructors
}
