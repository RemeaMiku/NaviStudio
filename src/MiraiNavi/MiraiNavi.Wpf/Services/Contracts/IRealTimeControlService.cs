using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IRealTimeControlService
{
    public bool IsStarted { get; }

    public bool IsRunning { get; }

    public void Start(RealTimeControlOptions options);

    public void Resume();

    public void Pause();

    public void Stop();
}
