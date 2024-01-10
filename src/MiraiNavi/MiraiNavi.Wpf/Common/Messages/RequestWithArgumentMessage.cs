using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MiraiNavi.WpfApp.Common.Messages;

public class RequestWithArgumentMessage<T, TResult>(T argument) : RequestMessage<TResult>
{
    public T Argument { get; } = argument;
}
