using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.Common.Messages;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.ViewModels.Windows;

public partial class MainWindowViewModel(IEpochDatasService epochDatasService, [FromKeyedServices("D:\\RemeaMiku study\\course in progress\\Graduation\\data\\机载.dts")] IRealTimeControlService realTimeControlService) : ObservableRecipient, IRecipient<RequestMessage<EpochData>>, IRecipient<RequestMessage<IEnumerable<EpochData>>>
{
    CancellationTokenSource? _tokenSource;
    readonly IEpochDatasService _epochDatasService = epochDatasService;
    readonly IRealTimeControlService _realTimeControlService = realTimeControlService;

    public static string Title => "MiraiNavi";

    protected override void OnActivated()
    {
        base.OnActivated();
        _realTimeControlService.EpochDataReceived += OnEpochDataReceived;
    }

    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        _realTimeControlService.EpochDataReceived -= OnEpochDataReceived;
    }

    private void OnEpochDataReceived(object? sender, EpochData? e)
    {
        if (e is null)
        {
            Messenger.Send(new Output(UtcTime.Now, Title, InfoBarSeverity.Warning, "数据异常"));
            return;
        }
        _epochDatasService.Add(e);
        Messenger.Send(new Output(UtcTime.Now, Title, InfoBarSeverity.Success, $"已解算第 {_epochDatasService.Datas.Count} 个历元数据：{e.TimeStamp:yyyy/MM/dd HH:mm:ss.fff}"));
        if (IsRealTimeRunning)
            Messenger.Send(e);
    }

    public static readonly RealTimeControlOptions DefaultOptions = new("默认");

    [ObservableProperty]
    RealTimeControlOptions? _realTimeControlOptions;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StartOrResumeCommand))]
    [NotifyPropertyChangedFor(nameof(StartOrResumeText))]
    bool _isRealTimeStarted;

    [ObservableProperty]
    bool _isRealTimeRunning;

    partial void OnIsRealTimeRunningChanged(bool value)
    {
        if (value)
            Messenger.Send(new NotificationMessage(NotificationType.Sync));
    }

    public string StartOrResumeText
        => IsRealTimeStarted ? "继续" : RealTimeControlOptions is null ? string.Empty : RealTimeControlOptions.Name;

    public IRelayCommand StartOrResumeCommand
        => IsRealTimeStarted ? ResumeCommand : StartCommand;

    [RelayCommand]
    async Task StartAsync()
    {
        if (IsRealTimeStarted)
            return;
        ArgumentNullException.ThrowIfNull(RealTimeControlOptions);
        _epochDatasService.Clear();
        _tokenSource = new();
        IsRealTimeStarted = true;
        IsRealTimeRunning = true;
        var notificationMessage = Messenger.Send(new NotificationMessage(NotificationType.Reset));
        Messenger.Send(new Output(notificationMessage.TimeStamp, Title, InfoBarSeverity.Informational, "开始解算"));
        try
        {
            await _realTimeControlService.StartListeningAsync(RealTimeControlOptions, _tokenSource.Token);
        }
        catch (Exception ex)
        {
            Trace.TraceError(ex.ToString());
            Messenger.Send(new Output(UtcTime.Now, Title, InfoBarSeverity.Error, $"发生致命错误，解算中止：{Environment.NewLine}{ex.Message}"));
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
        Messenger.Send(new Output(UtcTime.Now, Title, InfoBarSeverity.Informational, "继续更新"));
    }

    [RelayCommand]
    void Pause()
    {
        if (!IsRealTimeRunning)
            return;
        IsRealTimeRunning = false;
        Messenger.Send(new Output(UtcTime.Now, Title, InfoBarSeverity.Informational, "暂停更新"));
    }

    [RelayCommand]
    void Stop()
    {
        if (!IsRealTimeStarted)
            return;
        ArgumentNullException.ThrowIfNull(_tokenSource);
        _tokenSource.Cancel();
        IsRealTimeStarted = false;
        Messenger.Send(new Output(UtcTime.Now, Title, InfoBarSeverity.Informational, "停止解算"));
    }

    public void Receive(RequestMessage<EpochData> message)
    {
        if (_epochDatasService.Datas.Count > 0)
            message.Reply(_epochDatasService.Datas[^1]);
    }

    public void Receive(RequestMessage<IEnumerable<EpochData>> message)
    {
        if (_epochDatasService.Datas.Count > 0)
            message.Reply(_epochDatasService.Datas);
    }
}
