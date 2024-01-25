using System.IO.Ports;
using System.Net;

namespace MiraiNavi.Shared.Models.Options;

public class RealTimeOptions
{
    #region Public Constructors

    public RealTimeOptions() { }

    public RealTimeOptions(string name)
    {
        Name = name;
    }

    #endregion Public Constructors

    #region Public Properties

    public string Name { get; set; } = string.Empty;

    public InputOptions BaseOptions { get; set; } = new();

    public InputOptions RoverOptions { get; set; } = new();

    public string OutputFolder { get; set; } = string.Empty;

    #endregion Public Properties
}
