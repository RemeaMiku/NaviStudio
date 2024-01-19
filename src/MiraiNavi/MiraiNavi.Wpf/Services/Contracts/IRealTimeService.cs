using System.Threading;
using System.Threading.Tasks;
using MiraiNavi.Shared.Models.Options;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IRealTimeService
{
    public bool IsRunning { get; }

    public event EventHandler<EpochData?>? EpochDataReceived;
    //TODO 超时处理
    public Task StartAsync(RealTimeOptions options, CancellationToken token);
}
