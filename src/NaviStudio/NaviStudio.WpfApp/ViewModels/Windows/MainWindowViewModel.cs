using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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
    void SaveEpochDatas()
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
        try
        {
            StatusContent = "正在保存历元数据...";
            StatusSeverityType = SeverityType.Info;
            StatusIsProcessing = true;
            _epochDatasService.Save(dialog.FileName);
            var message = $"历元数据已保存至 {dialog.FileName}";
            _snackbarService.ShowSuccess("保存完成", message);
            StatusContent = "保存完成";
            StatusSeverityType = SeverityType.Success;
            Messenger.Send(new Output(Title, SeverityType.Info, message));
        }
        catch(Exception ex)
        {
            _snackbarService.ShowError("保存失败", $"保存历元数据时出错：{ex.Message}");
            StatusContent = "保存失败";
            StatusSeverityType = SeverityType.Error;
        }
        finally
        {
            StatusIsProcessing = false;
        }
    }

    [RelayCommand]
    async Task LoadEpochDatasAsync()
    {
        var dialog = new OpenFileDialog()
        {
            Title = "打开历元数据文件",
            Filter = $"NaviStudio 历元数据文件|*{FileExtensions.EpochDataFileExtension}|所有文件|*.*",
            RestoreDirectory = true,
            CheckFileExists = true,
        };
        if(dialog.ShowDialog() != true)
            return;
        try
        {
            StatusContent = "正在加载历元数据...";
            StatusSeverityType = SeverityType.Info;
            StatusIsProcessing = true;
            await Task.Run(() => _epochDatasService.Load(dialog.FileName));
            var message = $"已加载 {dialog.FileName}";
            Messenger.Send(NotificationMessage.Sync);
            Messenger.Send(new Output(Title, SeverityType.Info, message));
            StatusContent = "加载完成";
            StatusSeverityType = SeverityType.Success;
        }
        catch(Exception)
        {
            Messenger.Send(NotificationMessage.Reset);
            _snackbarService.ShowError("加载失败", "加载历元数据失败。可能不是目标文件格式。");
            StatusContent = "加载失败";
            StatusSeverityType = SeverityType.Error;
        }
        finally
        {
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
            Messenger.Send(NotificationMessage.Update);
            StatusSeverityType = SeverityType.Info;
            StatusContent = $"历元 {data.TimeStamp:yyyy/MM/dd HH:mm:ss.fff} 已更新";
        }
    }
    partial void OnIsRealTimeStartedChanged(bool value)
    {
        if(value)
        {
            Messenger.Send(NotificationMessage.Reset);
            Messenger.Send(new Output(Title, SeverityType.Info, "开始接收"));
            _realTimeControlService.EpochDataReceived += OnEpochDataReceived;
        }
        else
        {
            _realTimeControlService.EpochDataReceived -= OnEpochDataReceived;
            Messenger.Send(new Output(Title, SeverityType.Info, "停止接收"));
            Messenger.Send(NotificationMessage.Stop);
        }
    }

    partial void OnIsRealTimeRunningChanged(bool value)
    {
        if(value)
        {
            Messenger.Send(NotificationMessage.Sync);
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
        IsRealTimeStarted = true;
        _epochDatasService.Clear();
        IsRealTimeRunning = true;
        _tokenSource = new();
        try
        {
            if(!string.IsNullOrEmpty(Options.OutputFolder))
            {
                Directory.CreateDirectory(Options.OutputFolder);
                _epochDatasService.StartAutoSave(Path.Combine(Options.OutputFolder, $"{UtcTime.Now:yyMMddHHmmss}{FileExtensions.EpochDataFileExtension}"));
            }
            await _realTimeControlService.StartAsync(Options, _tokenSource.Token);
        }
        catch(Exception ex)
        {
            Trace.TraceError(ex.Message);
            Messenger.Send(new Output(Title, SeverityType.Error, $"出错了：{Environment.NewLine}{ex.Message}", ex.StackTrace));
        }
        finally
        {
            _epochDatasService.StopAutoSave();
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
        await Task.Delay(100);
        StartCommand.Execute(default);
    }

    [RelayCommand]
    void ClearEpochDatas()
    {
        if(!_epochDatasService.HasData)
        {
            _snackbarService.ShowError("清除失败", "无数据");
            return;
        }
        _epochDatasService.Clear();
        Messenger.Send(NotificationMessage.Reset);
        Messenger.Send(new Output(Title, SeverityType.Info, "历元数据已清除"));
        StatusContent = "历元数据已清除";
        StatusSeverityType = SeverityType.Info;
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
