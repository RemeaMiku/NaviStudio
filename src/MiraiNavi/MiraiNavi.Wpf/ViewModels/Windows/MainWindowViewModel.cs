using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MiraiNavi.WpfApp.Common.Messages;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Windows;

public partial class MainWindowViewModel(IEpochDatasService epochDatasService, IRealTimeControlService realTimeControlService) : ObservableRecipient, IRecipient<RequestMessage<EpochData>>, IRecipient<RequestMessage<IEnumerable<EpochData>>>
{
    CancellationTokenSource? _tokenSource;
    readonly IEpochDatasService _epochDatasService = epochDatasService;
    readonly IRealTimeControlService _realTimeControlService = realTimeControlService;

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

    private void OnEpochDataReceived(object? sender, EpochData e)
    {
        _epochDatasService.Add(e);
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
        Messenger.Send(new NotificationMessage(NotificationType.Reset));
        _tokenSource = new();
        IsRealTimeStarted = true;
        IsRealTimeRunning = true;
        try
        {
            await _realTimeControlService.StartListeningAsync(RealTimeControlOptions, _tokenSource.Token);
        }
        catch (Exception ex)
        {
            Trace.TraceError(ex.ToString());
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
    }

    [RelayCommand]
    void Pause()
    {
        if (!IsRealTimeRunning)
            return;
        IsRealTimeRunning = false;
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
