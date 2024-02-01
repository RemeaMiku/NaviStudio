namespace NaviStudio.WpfApp.Common.Messaging.Messages;

public class RealTimeNotification
{
    #region Public Fields

    public static readonly RealTimeNotification Reset = new();
    public static readonly RealTimeNotification Update = new();
    public static readonly RealTimeNotification Sync = new();

    #endregion Public Fields

    #region Private Constructors

    private RealTimeNotification() { }

    #endregion Private Constructors
}
