using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.Shared.Models.Options;

public class SerialPortOptions
{
    public string PortName { get; set; } = string.Empty;

    public int BaudRate { get; set; } = 9600;

    public Parity Parity { get; set; } = Parity.None;

    public int DataBits { get; set; } = 8;

    public StopBits StopBits { get; set; } = StopBits.One;

    public bool RtsEnable { get; set; } = false;
}
