using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MiraiNavi.WpfApp.Common.Messages;

public class RequestWithArgumentMessage<T, TResult>(T argument) : RequestMessage<TResult>
{
    public T Argument { get; } = argument;
}
