using System.Threading;
using System.Threading.Tasks;
using NaviStudio.Shared.Models.Options;

namespace NaviStudio.WpfApp.Services.Contracts;

public interface IRealTimeService
{
    #region Public Events

    public event EventHandler<EpochData?>? EpochDataReceived;

    #endregion Public Events

    #region Public Properties

    public bool IsRunning { get; }

    #endregion Public Properties

    #region Public Methods

    public Task StartAsync(RealTimeOptions options, CancellationToken token);

    #endregion Public Methods
}
