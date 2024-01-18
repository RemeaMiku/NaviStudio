using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.Shared.Models.Options;

public class InputOptions
{
    public InputType Type { get; set; } = InputType.Tcp;

    public InputFormat Format { get; set; } = InputFormat.RTCM3;

    public IPEndPointOptions TcpOptions { get; set; } = new();

    public SerialPortOptions SerialOptions { get; set; } = new();
}
