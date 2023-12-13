using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Models;

public readonly struct Output(InfoBarSeverity severity, string message)
{
    public UtcTime TimeStamp { get; } = UtcTime.Now;
    public InfoBarSeverity Severity { get; init; } = severity;
    public string Message { get; init; } = message;
}
