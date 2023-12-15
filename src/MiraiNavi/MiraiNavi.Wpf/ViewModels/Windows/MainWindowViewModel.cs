using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Common;
using MiraiNavi.WpfApp.Common.Messages;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Windows;

public partial class MainWindowViewModel(IRealTimeControlService realTimeControlService) : ObservableRecipient, IRecipient<RealTimeControlMessage>
{
    readonly IRealTimeControlService _realTimeControlService = realTimeControlService;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StartOrResumeButtonText))]
    RealTimeControlOptions? _realTimeControlOptions = RealTimeControlOptions.Default;

    public string StartOrResumeButtonText
        => IsRealTimeStarted ? "继续" : RealTimeControlOptions?.Name ?? string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StartOrResumeButtonText))]
    bool _isRealTimeStarted;

    [ObservableProperty]
    bool _isRealTimeRunning;

    [RelayCommand]
    void StartOrResume()
    {
        if (IsRealTimeRunning)
            return;
        if (IsRealTimeStarted)
        {
            _realTimeControlService.Resume();
            return;
        }
        _realTimeControlService.Start(RealTimeControlOptions!);

    }

    [RelayCommand]
    void Pause()
    {
        if (!IsRealTimeRunning)
            return;
        _realTimeControlService.Pause();
    }

    [RelayCommand]
    void Stop()
    {
        if (!IsRealTimeStarted)
            return;
        _realTimeControlService.Stop();
    }

    public void Receive(RealTimeControlMessage message)
    {
        switch (message.Mode)
        {
            case RealTimeControlMode.Start:
                IsRealTimeStarted = true;
                IsRealTimeRunning = true;
                break;
            case RealTimeControlMode.Pause:
                IsRealTimeRunning = false;
                break;
            case RealTimeControlMode.Resume:
                IsRealTimeRunning = true;
                break;
            case RealTimeControlMode.Stop:
                IsRealTimeRunning = false;
                IsRealTimeStarted = false;
                break;
            case RealTimeControlMode.Complete:
                IsRealTimeRunning = false;
                IsRealTimeStarted = false;
                break;
            default:
                break;
        }
    }
}
