namespace NaviStudio.Shared.Models.Options;

public class InputOptions
{
    #region Public Properties

    public InputType Type { get; set; } = InputType.Tcp;

    public InputFormat Format { get; set; } = InputFormat.RTCM3;

    public BaseType BaseType { get; set; } = BaseType.Single;

    public int ZmqId { get; set; }

    public int ServerCycle { get; set; } = 1000;

    public string ImuType { get; set; } = "NULL";

    public int ImuRate { get; set; } = 100;

    public IPEndPointOptions TcpOptions { get; set; } = new();

    public SerialPortOptions SerialOptions { get; set; } = new();

    public NtripOptions NtripOptions { get; set; } = new();

    #endregion Public Properties
}
