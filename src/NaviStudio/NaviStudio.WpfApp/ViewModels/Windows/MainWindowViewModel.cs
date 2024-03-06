using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Win32;
using NaviStudio.Shared;
using NaviStudio.Shared.Models.Options;
using NaviStudio.WpfApp.Common.Extensions;
using NaviStudio.WpfApp.Common.Helpers;
using NaviStudio.WpfApp.Common.Messaging;
using NaviStudio.WpfApp.Common.Messaging.Messages;
using NaviStudio.WpfApp.Services.Contracts;
using Wpf.Ui.Mvvm.Contracts;

namespace NaviStudio.WpfApp.ViewModels.Windows;

public partial class MainWindowViewModel(IEpochDatasService epochDatasService, IRealTimeService realTimeControlService, IMessenger messenger, ISnackbarService snackbarService) : ObservableRecipient(messenger), IRecipient<ValueChangedMessage<RealTimeOptions>>, IRecipient<StatusNotification>
{
    #region Public Fields

    public const string Title = "NaviStudio";

    //public static readonly RealTimeOptions DefaultOptions = new("默认");

    #endregion Public Fields

    #region Public Properties

    public string StartOrResumeText
        => IsRealTimeStarted ? "继续" : Options is null ? "启动" : $"启动：{Options.Name}";

    public IRelayCommand StartOrResumeCommand
        => IsRealTimeStarted ? ResumeCommand : StartCommand;

    #endregion Public Properties

    #region Public Methods

    public void Receive(ValueChangedMessage<RealTimeOptions> message)
    {
        Options = message.Value;
    }

    public void Receive(StatusNotification message)
    {
        StatusSeverityType = message.Type;
        StatusContent = message.Content;
        StatusIsProcessing = message.IsProcessing;
    }

    #endregion Public Methods

    #region Private Fields

    const string _optionsNullStatusContent = "无解算配置";

    const string _optionsReadyStatusContent = "就绪";

    readonly IEpochDatasService _epochDatasService = epochDatasService;

    readonly IRealTimeService _realTimeControlService = realTimeControlService;

    CancellationTokenSource? _tokenSource;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StartOrResumeText))]
    RealTimeOptions? _options;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StartOrResumeCommand))]
    [NotifyPropertyChangedFor(nameof(StartOrResumeText))]
    bool _isRealTimeStarted;

    [ObservableProperty]
    bool _isRealTimeRunning;

    [ObservableProperty]
    SeverityType _statusSeverityType = SeverityType.Info;

    [ObservableProperty]
    string _statusContent = _optionsNullStatusContent;

    [ObservableProperty]
    bool _statusIsProcessing = false;

    readonly ISnackbarService _snackbarService = snackbarService;

    #endregion Private Fields

    #region Private Methods

    [RelayCommand]
    async Task SaveEpochDatasAsync()
    {
        if(!_epochDatasService.HasData)
        {
            _snackbarService.ShowError("保存失败", "无数据");
            return;
        }
        var dialog = new SaveFileDialog()
        {
            Title = "保存历元数据文件至",
            Filter = $"NaviStudio 历元数据文件|*{FileExtensions.EpochDataFileExtension}|所有文件|*.*",
            RestoreDirectory = true,
            CheckPathExists = true,
        };
        if(dialog.ShowDialog() != true)
            return;
        var json = JsonSerializer.Serialize(_epochDatasService.Datas);
        try
        {
            StatusContent = "正在保存历元数据...";
            StatusSeverityType = SeverityType.Info;
            StatusIsProcessing = true;
            await File.WriteAllTextAsync(dialog.FileName, json);
            var message = $"历元数据已保存至 {dialog.FileName}";
            _snackbarService.ShowSuccess("保存成功", message);
            StatusContent = message;
            StatusSeverityType = SeverityType.Success;
            StatusIsProcessing = false;
        }
        catch(Exception)
        {
            _snackbarService.ShowError("保存失败", "无法保存历元数据");
            StatusContent = "保存失败";
            StatusSeverityType = SeverityType.Error;
            StatusIsProcessing = false;
        }
    }

    void ValidateEpochData(EpochData epochData)
    {
        foreach(var info in typeof(EpochData).GetProperties())
        {
            if(info.GetValue(epochData) is null)
            {
                Messenger.Send(new Output(Title, SeverityType.Warning, $"{DisplayDescriptionManager.Descriptions[info.Name]}数据异常"));
            }
        }
    }

    void OnEpochDataReceived(object? sender, EpochData? data)
    {
        if(data is null)
        {
            Messenger.Send(new Output(Title, SeverityType.Warning, "数据异常"));
            StatusSeverityType = SeverityType.Warning;
            StatusContent = $"历元 {UtcTime.Now:yyyy/MM/dd HH:mm:ss.fff} 更新失败";
            return;
        }
        ValidateEpochData(data);
        _epochDatasService.Add(data);
        if(IsRealTimeRunning)
        {
            Messenger.Send(RealTimeNotification.Update);
            StatusSeverityType = SeverityType.Info;
            StatusContent = $"历元 {data.TimeStamp:yyyy/MM/dd HH:mm:ss.fff} 已更新";
        }
    }
    partial void OnIsRealTimeStartedChanged(bool value)
    {
        if(value)
        {
            Messenger.Send(RealTimeNotification.Reset);
            Messenger.Send(new Output(Title, SeverityType.Info, "开始接收"));
            _realTimeControlService.EpochDataReceived += OnEpochDataReceived;
        }
        else
        {
            _realTimeControlService.EpochDataReceived -= OnEpochDataReceived;
            Messenger.Send(new Output(Title, SeverityType.Info, "停止接收"));
            Messenger.Send(RealTimeNotification.Stop);
        }
    }

    partial void OnIsRealTimeRunningChanged(bool value)
    {
        if(value)
        {
            Messenger.Send(RealTimeNotification.Sync);
            if(_epochDatasService.HasData)
            {
                StatusSeverityType = SeverityType.Info;
                StatusContent = $"历元 {_epochDatasService.Last.TimeStamp:yyyy/MM/dd HH:mm:ss.fff} 已更新";
            }
            StatusIsProcessing = true;
        }
        else
            StatusIsProcessing = false;
        Messenger.Send(new ValueChangedMessage<bool>(value), MessageTokens.IsRealTimeRunning);
    }

    [RelayCommand]
    async Task StartAsync()
    {
        if(IsRealTimeStarted)
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
        catch(Exception ex)
        {
            Trace.TraceError(ex.Message);
            Messenger.Send(new Output(Title, SeverityType.Error, $"出错了：{Environment.NewLine}{ex.Message}", ex.StackTrace));
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
        if(!IsRealTimeStarted)
            return;
        IsRealTimeRunning = true;
        Messenger.Send(new Output(Title, SeverityType.Info, "继续更新"));
    }

    [RelayCommand]
    void Pause()
    {
        if(!IsRealTimeRunning)
            return;
        IsRealTimeRunning = false;
        Messenger.Send(new Output(Title, SeverityType.Info, "暂停更新"));
    }

    [RelayCommand]
    void Stop()
    {
        if(!IsRealTimeStarted)
            return;
        ArgumentNullException.ThrowIfNull(_tokenSource);
        _tokenSource.Cancel();
        IsRealTimeStarted = false;
    }

    [RelayCommand]
    async Task Restart()
    {
        if(!IsRealTimeStarted)
            return;
        Stop();
        await Task.Delay(500);
        StartCommand.Execute(default);
    }

    [RelayCommand]
    void ClearEpochDatas()
    {
        if(!_epochDatasService.HasData)
        {
            _snackbarService.ShowError("清空失败", "无数据");
            return;
        }
        _epochDatasService.Clear();
        Messenger.Send(RealTimeNotification.Reset);
        Messenger.Send(new Output(Title, SeverityType.Info, "历元数据已清空"));
    }

    partial void OnOptionsChanged(RealTimeOptions? value)
    {
        if(value is null)
        {
            StatusContent = _optionsNullStatusContent;
            StatusSeverityType = SeverityType.Info;
        }
        else
        {
            StatusContent = _optionsReadyStatusContent;
            StatusSeverityType = SeverityType.Success;
        }
    }

    #endregion Private Methods
}
