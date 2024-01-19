using System.IO.Ports;
using System.Net;

namespace MiraiNavi.Shared.Models.Options;

public class RealTimeOptions
{
    public RealTimeOptions() { }

    public RealTimeOptions(string name)
    {
        Name = name;
    }

    public string Name { get; set; } = string.Empty;

    public InputOptions BaseOptions { get; set; } = new();

    public InputOptions RoverOptions { get; set; } = new();
}
