using System.Threading;
using System.Threading.Tasks;
using MiraiNavi.Shared.Models.RealTime;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IRealTimeSolutionService
{
    public bool IsRunning { get; }

    public event EventHandler<EpochData?>? EpochDataReceived;
    //TODO 超时处理
    public Task StartAsync(RealTimeSolutionOptions options, CancellationToken token);
}
