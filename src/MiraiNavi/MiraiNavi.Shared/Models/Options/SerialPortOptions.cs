using System.IO.Ports;

namespace MiraiNavi.Shared.Models.Options;

public class SerialPortOptions
{
    #region Public Properties

    public string PortName { get; set; } = string.Empty;

    public int BaudRate { get; set; } = 9600;

    public Parity Parity { get; set; } = Parity.None;

    public int DataBits { get; set; } = 8;

    public StopBits StopBits { get; set; } = StopBits.One;

    public bool RtsEnable { get; set; } = false;

    #endregion Public Properties
}
