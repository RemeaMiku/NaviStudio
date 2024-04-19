using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Win32;
using NaviStudio.Shared;
using NaviStudio.Shared.Models.Options;
using NaviStudio.WpfApp.Common.Extensions;
using NaviStudio.WpfApp.Common.Messaging.Messages;
using Wpf.Ui.Mvvm.Contracts;

namespace NaviStudio.WpfApp.ViewModels.Pages;

public partial class RealTimeOptionsPageViewModel : ObservableValidator, IRecipient<NotificationMessage>
{
    #region Public Fields

    public const string Title = "实时解算配置";
    public const string MenuItemHeader = $"{Title}(_C)";

    #endregion Public Fields

    #region Public Constructors

    public RealTimeOptionsPageViewModel(IMessenger messenger, ISnackbarService snackbarService)
    {
        _messenger = messenger;
        _messenger.Register(this);
        _snackbarService = snackbarService;
        BaseOptions = new();
        RoverOptions = new();
        PropertyChanged += (_, e) =>
        {
            if(e.PropertyName is not (nameof(HasChanged) or nameof(IsEditable)))
                HasChanged = true;
        };
        if(File.Exists(_cachedOptionsFileName))
            Read(_cachedOptionsFileName);
    }

    #endregion Public Constructors

    #region Public Properties

    public static int[] ServerCycles { get; } = [1000, 20, 10, 5];

    public static string[] ImuTypes { get; } = ["NULL", "NovAtel SPAN FSAS"];

    public static int[] ImuRates { get; } = [100, 125, 200];

    public static Array Parities { get; } = Enum.GetValues(typeof(Parity));

    public static Array InputFormats { get; } = Enum.GetValues(typeof(InputFormat));

    public static int[] DataBits { get; } = [7, 8];

    public static IEnumerable<int> BaudRates { get; } = [2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600];

    public new bool HasErrors => base.HasErrors || BaseOptions.HasErrors || RoverOptions.HasErrors;

    #endregion Public Properties

    #region Public Methods

    public RealTimeOptions GetOptions()
    {
        return new RealTimeOptions()
        {
            Name = SolutionName,
            BaseOptions = BaseOptions,
            RoverOptions = RoverOptions,
            OutputFolder = OutputFolder,
        };
    }

    #endregion Public Methods

    #region Private Fields    

    [ObservableProperty]
    bool _hasChanged = true;

    readonly static JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = false,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
        PreferredObjectCreationHandling = JsonObjectCreationHandling.Populate,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters =
        {
            new JsonStringEnumConverter(),
        }
    };

    readonly IMessenger _messenger;
    readonly ISnackbarService _snackbarService;

    [ObservableProperty]
    [Required(ErrorMessage = "不能为空")]
    [Length(1, 20, ErrorMessage = "长度需在 1 到 20 之间")]
    [NotifyDataErrorInfo]
    string _solutionName = "默认";

    [ObservableProperty]
    InputOptions _baseOptions;

    [ObservableProperty]
    InputOptions _roverOptions;

    [ObservableProperty]
    [RegularExpression(@"^(?:[\w]\:|\\)(\\[a-zA-Z_\-\s0-9\.]+)+\\?$", ErrorMessage = "非法目录")]
    [NotifyDataErrorInfo]
    string _outputFolder = string.Empty;

    [ObservableProperty]
    bool _isEditable = true;

    const string _cachedOptionsFileName = $"cache{FileExtensions.RealTimeOptionsFileExtension}";

    #endregion Private Fields

    #region Private Methods


    partial void OnBaseOptionsChanged(InputOptions value) =>
        OnInputOptionsChanged(value);

    partial void OnRoverOptionsChanged(InputOptions value) =>
        OnInputOptionsChanged(value);

    void OnInputOptionsChanged(InputOptions options)
    {
        options.PropertyChanged += (_, e) =>
        {
            if(e.PropertyName == nameof(InputOptions.HasErrors))
                OnPropertyChanged(nameof(HasErrors));
            HasChanged = true;
        };
    }

    void SetOptions(RealTimeOptions options)
    {
        SolutionName = options.Name;
        BaseOptions = options.BaseOptions;
        RoverOptions = options.RoverOptions;
        OutputFolder = options.OutputFolder;
    }

    [RelayCommand]
    void SelectOuputFolder()
    {
        var dialog = new OpenFolderDialog()
        {
            Title = "选择输出目录",
            Multiselect = false,
        };
        if(dialog.ShowDialog() == true)
            OutputFolder = dialog.FolderName;
    }

    [RelayCommand]
    void Read(string? filePath = default)
    {
        if(filePath is null)
        {
            var dialog = new OpenFileDialog()
            {
                Title = "打开解算配置文件",
                Filter = $"NaviStudio 解算配置文件|*{FileExtensions.RealTimeOptionsFileExtension}|所有文件|*.*",
                RestoreDirectory = true,
                CheckPathExists = true,
            };
            if(dialog.ShowDialog() != true)
                return;
            filePath = dialog.FileName;
        }
        try
        {
            var content = File.ReadAllText(filePath, Encoding.UTF8);
            var options = JsonSerializer.Deserialize<RealTimeOptions>(content, _jsonSerializerOptions);
            if(options is null)
                return;
            SetOptions(options);
        }
        catch(JsonException)
        {
            _snackbarService.ShowError("读取出错", $"{filePath} 不是有效的解算配置文件");
        }
        catch(Exception e)
        {
            _snackbarService.ShowError("读取出错", e.Message);
        }
    }

    [RelayCommand]
    void Save()
    {
        var builder = new StringBuilder(SolutionName);
        foreach(var ch in Path.GetInvalidFileNameChars())
            builder.Replace(ch, '_');
        var fileName = $"{builder}{FileExtensions.RealTimeOptionsFileExtension}";
        var dialog = new SaveFileDialog()
        {
            Title = "保存解算配置文件至",
            Filter = $"NaviStudio 解算预设文件|*{FileExtensions.RealTimeOptionsFileExtension}|所有文件|*.*",
            FileName = fileName,
            RestoreDirectory = true,
            CheckPathExists = true,
        };
        if(dialog.ShowDialog() != true)
            return;
        try
        {
            var content = JsonSerializer.Serialize(GetOptions(), _jsonSerializerOptions);
            File.WriteAllText(dialog.FileName, content);
            _snackbarService.ShowSuccess("保存成功", $"解算配置已保存至 {dialog.FileName}");
        }
        catch(Exception e)
        {
            _snackbarService.ShowError("保存出错", e.Message);
        }
    }

    [RelayCommand]
    void Confirm()
    {
        if(!HasChanged)
            return;
        var options = GetOptions();
        var content = JsonSerializer.Serialize(options, _jsonSerializerOptions);
        File.WriteAllText(_cachedOptionsFileName, content);
        _messenger.Send(new ValueChangedMessage<RealTimeOptions>(GetOptions()));
        _messenger.Send(new Output(Title, SeverityType.Info, "解算配置已更新"));
        HasChanged = false;
    }

    public void Receive(NotificationMessage message)
    {
        if(message == NotificationMessage.Start)
            IsEditable = false;
        else if(message == NotificationMessage.Stop)
            IsEditable = true;
    }

    #endregion Private Methods
}
