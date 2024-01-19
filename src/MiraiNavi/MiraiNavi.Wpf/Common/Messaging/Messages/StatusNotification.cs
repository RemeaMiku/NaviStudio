using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Common.Messaging.Messages;

public class StatusNotification(string content, SeverityType type, bool isProcessing = false)
{
    public SeverityType Type { get; set; } = type;

    public bool IsProcessing { get; set; } = isProcessing;

    public string Content { get; set; } = content;
}
