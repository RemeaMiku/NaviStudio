using System.IO.Ports;
using System.Net;

namespace MiraiNavi.Shared.Models.Solution;

public class SolutionOptions(string name)
{
    public string Name { get; set; } = name;

    public InputOptions BaseOptions { get; set; } = InputOptions.DefaultBaseOptions;

    public InputOptions RoverOptions { get; set; } = new();

    public int MaxEpochCount { get; set; } = int.MaxValue;
}
