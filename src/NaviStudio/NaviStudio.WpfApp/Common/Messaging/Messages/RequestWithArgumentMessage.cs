using CommunityToolkit.Mvvm.Messaging.Messages;

namespace NaviStudio.WpfApp.Common.Messaging.Messages;

public class RequestWithArgumentMessage<T, TResult>(T argument) : RequestMessage<TResult>
{
    #region Public Properties

    public T Argument { get; } = argument;

    #endregion Public Properties
}