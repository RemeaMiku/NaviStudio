using System.Threading;
using System.Threading.Tasks;
using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IRealTimeControlService
{
    public bool IsRunning { get; }

    public event EventHandler<EpochData>? EpochDataReceived;

    public Task StartListeningAsync(RealTimeControlOptions options, CancellationToken token);
}
