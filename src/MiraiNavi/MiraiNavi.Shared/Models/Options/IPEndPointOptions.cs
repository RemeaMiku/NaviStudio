using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.Shared.Models.Options;

public class IPEndPointOptions
{
    public string Address { get; set; } = IPAddress.None.ToString();

    public int Port { get; set; } = IPEndPoint.MaxPort;

    public IPEndPoint ToIPEndPoint() => new(IPAddress.Parse(Address), Port);

    public IPEndPointOptions() { }

    public IPEndPointOptions(string address, int port)
    {
        Address = address;
        Port = port;
    }
}
