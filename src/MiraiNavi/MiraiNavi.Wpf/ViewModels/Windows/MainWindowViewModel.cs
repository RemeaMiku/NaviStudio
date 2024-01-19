using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MiraiNavi.Shared.Models.Options;
using MiraiNavi.WpfApp.Common.Helpers;
using MiraiNavi.WpfApp.Common.Messaging;
using MiraiNavi.WpfApp.Models.Chart;
using MiraiNavi.WpfApp.Models.Output;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Windows;

public partial class MainWindowViewModel(IEpochDatasService epochDatasService, IRealTimeService realTimeControlService, IMessenger messenger) : ObservableRecipient(messenger), IRecipient<ValueChangedMessage<RealTimeOptions>>
{
    CancellationTokenSource? _tokenSource;
    readonly IEpochDatasService _epochDatasService = epochDatasService;
    readonly IRealTimeService _realTimeControlService = realTimeControlService;

    public const string Title = "MiraiNavi";

    void ValidateEpochData(EpochData epochData)
    {
        foreach (var info in typeof(EpochData).GetProperties())
        {
            if (info.GetValue(epochData) is null)
            {
                Messenger.Send(new Output(Title, OutputType.Warning, $"{DisplayDescriptionManager.Descriptions[info.Name]}数据异常"));
            }
        }
    }

    void OnEpochDataReceived(object? sender, EpochData? data)
    {
        if (data is null)
        {
            Messenger.Send(new Output(Title, OutputType.Warning, "数据异常"));
            return;
        }
        ValidateEpochData(data);
        _epochDatasService.Add(data);
        if (IsRealTimeRunning)
            Messenger.Send(new NotificationMessage(Notifications.Update));
    }

    public static readonly RealTimeOptions DefaultOptions = new("默认");

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StartOrResumeText))]
    RealTimeOptions? _options;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StartOrResumeCommand))]
    [NotifyPropertyChangedFor(nameof(StartOrResumeText))]
    bool _isRealTimeStarted;

    [ObservableProperty]
    bool _isRealTimeRunning;

    partial void OnIsRealTimeStartedChanged(bool value)
    {
        if (value)
        {
            Messenger.Send(new NotificationMessage(Notifications.Reset));
            Messenger.Send(new Output(Title, OutputType.Info, "开始接收"));
            _realTimeControlService.EpochDataReceived += OnEpochDataReceived;
        }
        else
        {
            _realTimeControlService.EpochDataReceived -= OnEpochDataReceived;
            Messenger.Send(new Output(Title, OutputType.Info, "停止接收"));
        }
    }

    partial void OnIsRealTimeRunningChanged(bool value)
    {
        if (value)
            Messenger.Send(new NotificationMessage(Notifications.Sync));
    }

    public string StartOrResumeText
        => IsRealTimeStarted ? "继续" : Options is null ? string.Empty : Options.Name;

    public IRelayCommand StartOrResumeCommand
        => IsRealTimeStarted ? ResumeCommand : StartCommand;

    [RelayCommand]
    async Task StartAsync()
    {
        if (IsRealTimeStarted)
            return;
        ArgumentNullException.ThrowIfNull(Options);
        _epochDatasService.Clear();
        _tokenSource = new();
        IsRealTimeStarted = true;
        IsRealTimeRunning = true;
        try
        {
            await _realTimeControlService.StartAsync(Options, _tokenSource.Token);
        }
        catch (Exception ex)
        {
            Trace.TraceError(ex.Message);
            Messenger.Send(new Output(Title, OutputType.Error, $"出错了：{Environment.NewLine}{ex.Message}", ex.StackTrace));
        }
        finally
        {
            _tokenSource.Dispose();
            _tokenSource = default;
            IsRealTimeRunning = false;
            IsRealTimeStarted = false;
        }
    }

    [RelayCommand]
    void Resume()
    {
        if (!IsRealTimeStarted)
            return;
        IsRealTimeRunning = true;
        Messenger.Send(new Output(Title, OutputType.Info, "继续更新"));
    }

    [RelayCommand]
    void Pause()
    {
        if (!IsRealTimeRunning)
            return;
        IsRealTimeRunning = false;
        Messenger.Send(new Output(Title, OutputType.Info, "暂停更新"));
    }

    [RelayCommand]
    void Stop()
    {
        if (!IsRealTimeStarted)
            return;
        ArgumentNullException.ThrowIfNull(_tokenSource);
        _tokenSource.Cancel();
        IsRealTimeStarted = false;
    }

    public void Receive(ValueChangedMessage<RealTimeOptions> message)
    {
        Options = message.Value;
    }
}
