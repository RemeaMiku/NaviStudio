using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.Shared.Models.Solution;

public class InputOptions
{
    public const string DefaultSerialPortName = "COM1";

    public InputType Type { get; set; } = InputType.TCP;

    public InputFormat Format { get; set; } = InputFormat.RTCM3;

    public IPEndPoint TcpEndPoint { get; set; } = new(IPAddress.None, IPEndPoint.MaxPort);

    public SerialPort SerialPort { get; set; } = new() { PortName = DefaultSerialPortName };

    public static readonly InputOptions DefaultBaseOptions = new()
    {
        Type = InputType.TCP,
        Format = InputFormat.RTCM3,
    };
}
