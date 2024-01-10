using System.Threading;
using System.Threading.Tasks;
using MiraiNavi.Shared.Models.Solution;
using MiraiNavi.Shared.Models.Solution.RealTime;
using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IRealTimeControlService
{
    public bool IsRunning { get; }

    public event EventHandler<EpochData?>? EpochDataReceived;

    public Task StartListeningAsync(RealTimeSolutionOptions options, CancellationToken token);
}
