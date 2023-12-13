using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IRealTimeControlService
{
    public bool IsRunning { get; }

    public void Start();

    public void Pause();

    public void Stop();
}
