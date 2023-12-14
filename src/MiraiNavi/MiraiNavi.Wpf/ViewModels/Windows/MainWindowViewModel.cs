using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiraiNavi.WpfApp.Common;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Windows;

public partial class MainWindowViewModel(IRealTimeControlService realTimeControlService) : BaseViewModel
{
    readonly IRealTimeControlService _realTimeControlService = realTimeControlService;

    [ObservableProperty]
    RealTimeControlOptions? _realTimeControlOptions = RealTimeControlOptions.Default;

    [ObservableProperty]
    bool _isRealTimeStarted;

    [ObservableProperty]
    bool _isRealTimeRunning;

    [RelayCommand]
    async Task StartOrResumeAsync()
    {
        if (IsRealTimeRunning)
            return;
        if (IsRealTimeStarted)
        {
            IsRealTimeRunning = true;
            _realTimeControlService.Resume();
            return;
        }
        IsRealTimeStarted = true;
        IsRealTimeRunning = true;
        await _realTimeControlService.StartAsync(RealTimeControlOptions!);
        IsRealTimeStarted = false;
    }

    [RelayCommand]
    void Pause()
    {
        if (!IsRealTimeRunning)
            return;
        IsRealTimeRunning = false;
        _realTimeControlService.Pause();
    }

    [RelayCommand]
    void Stop()
    {
        if (!IsRealTimeStarted)
            return;
        IsRealTimeRunning = false;
        IsRealTimeStarted = false;
        _realTimeControlService.Stop();
    }
}
