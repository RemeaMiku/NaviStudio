using System.IO.Ports;
using System.Net;

namespace MiraiNavi.Shared.Models.Options;

public class SolutionOptions
{
    public SolutionOptions() { }

    public SolutionOptions(string name)
    {
        Name = name;
    }

    public string Name { get; set; } = string.Empty;

    public InputOptions BaseOptions { get; set; } = new();

    public InputOptions RoverOptions { get; set; } = new();

    public string OutputFilePath { get; set; } = string.Empty;
}
