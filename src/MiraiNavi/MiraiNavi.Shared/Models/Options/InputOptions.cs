namespace MiraiNavi.Shared.Models.Options;

public class InputOptions
{
    #region Public Properties

    public InputType Type { get; set; } = InputType.Tcp;

    public InputFormat Format { get; set; } = InputFormat.RTCM3;

    public IPEndPointOptions TcpOptions { get; set; } = new();

    public SerialPortOptions SerialOptions { get; set; } = new();

    #endregion Public Properties
}
