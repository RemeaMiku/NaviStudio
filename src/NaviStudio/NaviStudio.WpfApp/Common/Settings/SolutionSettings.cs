using System.Net;
using NaviStudio.Shared.Models.Options;

namespace NaviStudio.WpfApp.Common.Settings;

public class SolutionSettings
{
    public const int DefaultEpochDataTcpPort = 39831;

    #region Public Properties

    public IPEndPointOptions EpochDataTcpOptions { get; set; } = new("127.0.0.1", DefaultEpochDataTcpPort);

    public double Timeout { get; set; } = 15;

    public bool IsSolverEnabled { get; set; } = true;

    public List<string> SolverProcessPaths { get; set; } = [];

    public string BaseOptionFilePath { get; set; } = string.Empty;

    public string RoverOptionFilePath { get; set; } = string.Empty;

    #endregion Public Properties
}
