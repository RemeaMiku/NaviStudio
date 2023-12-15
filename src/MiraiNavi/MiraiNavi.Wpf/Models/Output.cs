﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Models;

public class Output(string senderName, InfoBarSeverity severity, string message)
{
    public UtcTime TimeStamp { get; } = UtcTime.Now;

    public string SenderName { get; init; } = senderName;

    public InfoBarSeverity Severity { get; init; } = severity;

    public string Message { get; init; } = message;
}
