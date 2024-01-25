using System.Net;
using MiraiNavi.Shared.Models.Options;

namespace MiraiNavi.WpfApp.Common.Settings;

public class SolutionSettings
{
    #region Public Fields

    public static readonly IPEndPointOptions DefaultEpochDataIPEndPointOptions = new(IPAddress.Loopback.ToString(), 39831);

    #endregion Public Fields

    #region Public Properties

    public IPEndPointOptions EpochDataTcpOptions { get; set; } = DefaultEpochDataIPEndPointOptions;

    #endregion Public Properties
}
