using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiraiNavi.WpfApp.Common;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IRealTimeControlService
{
    public bool IsStarted { get; }

    public bool IsRunning { get; }

    public Task StartAsync(RealTimeControlOptions options);

    public void Resume();

    public void Pause();

    public void Stop();
}
