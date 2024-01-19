using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Common.Messaging.Messages;

public class RealTimeNotification
{
    public static readonly RealTimeNotification Reset = new();
    public static readonly RealTimeNotification Update = new();
    public static readonly RealTimeNotification Sync = new();

    private RealTimeNotification() { }
}
