using System.Threading;
using System.Threading.Tasks;
using MiraiNavi.Shared.Models.Options;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IRealTimeService
{
    #region Public Events

    public event EventHandler<EpochData?>? EpochDataReceived;

    #endregion Public Events

    #region Public Properties

    public bool IsRunning { get; }

    #endregion Public Properties

    #region Public Methods

    //TODO 超时处理
    public Task StartAsync(RealTimeOptions options, CancellationToken token);

    #endregion Public Methods
}
