using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.Shared.Models.Solution;
using MiraiNavi.WpfApp.Common.Helpers;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Windows;

public partial class MainWindowViewModel(IEpochDatasService epochDatasService, IRealTimeSolutionService realTimeControlService, IMessenger messenger) : ObservableObject
{
    CancellationTokenSource? _tokenSource;
    readonly IEpochDatasService _epochDatasService = epochDatasService;
    readonly IRealTimeSolutionService _realTimeControlService = realTimeControlService;
    readonly IMessenger _messenger = messenger;

    public const string Title = "MiraiNavi";

    void ValidateEpochData(EpochData epochData)
    {
        foreach (var info in typeof(EpochData).GetProperties())
        {
            if (info.GetValue(epochData) is null)
            {
                _messenger.Send(new Output(Title, OutputType.Warning, $"{DisplayDescriptionManager.Descriptions[info.Name]}数据异常"));
            }
        }
    }

    void OnEpochDataReceived(object? sender, EpochData? data)
    {
        if (data is null)
        {
            _messenger.Send(new Output(Title, OutputType.Warning, "数据异常"));
            return;
        }
        ValidateEpochData(data);
        _epochDatasService.Add(data);
        if (IsRealTimeRunning)
            _messenger.Send(new NotificationMessage(Notifications.Update));
    }

    public static readonly SolutionOptions DefaultOptions = new("默认");

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StartOrResumeText))]
    SolutionOptions? _options;

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
            _messenger.Send(new NotificationMessage(Notifications.Reset));
            _messenger.Send(new Output(Title, OutputType.Info, "解算开始"));
            _realTimeControlService.EpochDataReceived += OnEpochDataReceived;
        }
        else
        {
            _realTimeControlService.EpochDataReceived -= OnEpochDataReceived;
            _messenger.Send(new Output(Title, OutputType.Info, "解算停止"));
        }
    }

    partial void OnIsRealTimeRunningChanged(bool value)
    {
        if (value)
            _messenger.Send(new NotificationMessage(Notifications.Sync));
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
            _messenger.Send(new Output(Title, OutputType.Error, $"出错了，实时解算中止：{Environment.NewLine}{ex.Message}", ex.StackTrace));
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
        _messenger.Send(new Output(Title, OutputType.Info, "更新继续"));
    }

    [RelayCommand]
    void Pause()
    {
        if (!IsRealTimeRunning)
            return;
        IsRealTimeRunning = false;
        _messenger.Send(new Output(Title, OutputType.Info, "更新暂停"));
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
}
