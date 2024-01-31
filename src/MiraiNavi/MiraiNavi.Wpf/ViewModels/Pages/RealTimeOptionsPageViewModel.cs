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
using MiraiNavi.Shared.Models.Options;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class RealTimeOptionsPageViewModel : ObservableValidator, IRecipient<RealTimeOptions>
{
    #region Public Fields

    public const string Title = "实时选项";
    public const string MenuItemHeader = $"{Title}(_S)";

    #endregion Public Fields

    #region Public Constructors

    public RealTimeOptionsPageViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        _messenger.Register(this);
        BaseOptions.PropertyChanged += (_, _) => OnPropertyChanged(nameof(HasErrors));
        RoverOptions.PropertyChanged += (_, _) => OnPropertyChanged(nameof(HasErrors));
    }

    #endregion Public Constructors

    #region Public Properties

    public static Array Parities { get; } = Enum.GetValues(typeof(Parity));

    public static Array InputFormats { get; } = Enum.GetValues(typeof(InputFormat));

    public static int[] DataBits { get; } = [5, 6, 7, 8];

    public static IEnumerable<int> BaudRates { get; } = [2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600];

    public new bool HasErrors => base.HasErrors || BaseOptions.HasErrors || RoverOptions.HasErrors;

    #endregion Public Properties

    #region Public Methods

    public RealTimeOptions GetOptions()
    {
        return new RealTimeOptions()
        {
            Name = SolutionName,
            BaseOptions = BaseOptions.GetInputOptions(),
            RoverOptions = RoverOptions.GetInputOptions(),
            OutputFolder = OutputFolder,
        };
    }

    public bool TryGetSolutionOptions([NotNullWhen(true)] out RealTimeOptions? options)
    {
        if (HasErrors)
        {
            options = default;
            return false;
        }
        options = GetOptions();
        return true;
    }

    public void Receive(RealTimeOptions message)
    {
        IsEditable = !IsEditable;
        SetOptions(message);
    }

    #endregion Public Methods

    #region Private Fields

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
    [ObservableProperty]
    [Required(ErrorMessage = "不能为空")]
    [Length(1, 20, ErrorMessage = "长度需在 1 到 20 之间")]
    [NotifyDataErrorInfo]
    string _solutionName = "未命名";

    [ObservableProperty]
    InputOptionsViewModel _baseOptions = new();

    [ObservableProperty]
    InputOptionsViewModel _roverOptions = new();

    [ObservableProperty]
    [RegularExpression(@"^(?:[\w]\:|\\)(\\[a-zA-Z_\-\s0-9\.]+)+\\?$", ErrorMessage = "非法目录")]
    [NotifyDataErrorInfo]
    string _outputFolder = string.Empty;

    [ObservableProperty]
    bool _isEditable = true;

    #endregion Private Fields

    #region Private Methods

    partial void OnBaseOptionsChanged(InputOptionsViewModel? oldValue, InputOptionsViewModel newValue)
    {
        OnPropertyChanged(nameof(HasErrors));
        if (oldValue is not null)
            oldValue.ErrorsChanged -= (_, _) => OnPropertyChanged(nameof(HasErrors));
        newValue.ErrorsChanged += (_, _) => OnPropertyChanged(nameof(HasErrors));
    }

    partial void OnRoverOptionsChanged(InputOptionsViewModel? oldValue, InputOptionsViewModel newValue)
        => OnBaseOptionsChanged(oldValue, newValue);
    void SetOptions(RealTimeOptions options)
    {
        SolutionName = options.Name;
        BaseOptions = new InputOptionsViewModel(options.BaseOptions);
        RoverOptions = new InputOptionsViewModel(options.RoverOptions);
    }

    [RelayCommand]
    void SelectOuputFolder()
    {
        var dialog = new OpenFolderDialog()
        {
            Title = "输出历元数据文件至",
            Multiselect = false,
        };
        if (dialog.ShowDialog() == true)
            OutputFolder = dialog.FolderName;
    }

    [RelayCommand]
    void Read()
    {
        var dialog = new OpenFileDialog()
        {
            Title = "选择解算选项预设文件",
            Filter = "MiraiNavi 解算预设文件|*.mnso|所有文件|*.*",
            RestoreDirectory = true,
            CheckPathExists = true,
        };
        if (dialog.ShowDialog() != true)
            return;
        var content = File.ReadAllText(dialog.FileName, Encoding.UTF8);
        try
        {
            var options = JsonSerializer.Deserialize<RealTimeOptions>(content, _jsonSerializerOptions);
            if (options is null)
                return;
            SetOptions(options);
        }
        catch (Exception)
        {
            // TODO 异常处理
        }
    }

    [RelayCommand]
    void Save()
    {
        var builder = new StringBuilder(SolutionName);
        foreach (var ch in Path.GetInvalidFileNameChars())
            builder.Replace(ch, '_');
        var fileName = $"{builder}.mnso";
        var dialog = new SaveFileDialog()
        {
            Title = "保存解算选项预设至",
            Filter = "MiraiNavi 解算预设文件|*.mnso|所有文件|*.*",
            FileName = fileName,
            RestoreDirectory = true,
            CheckPathExists = true,
        };
        if (dialog.ShowDialog() != true)
            return;
        var content = JsonSerializer.Serialize(GetOptions(), _jsonSerializerOptions);
        File.WriteAllText(dialog.FileName, content, Encoding.UTF8);
    }

    [RelayCommand]
    void Confirm()
    {
        _messenger.Send(new ValueChangedMessage<RealTimeOptions>(GetOptions()));
    }

    #endregion Private Methods
}
