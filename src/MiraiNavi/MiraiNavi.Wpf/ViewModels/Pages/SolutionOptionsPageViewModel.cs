using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using MiraiNavi.Shared.Models.Options;
using MiraiNavi.WpfApp.Models;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Interfaces;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class SolutionOptionsPageViewModel : ObservableValidator
{
    public const string Title = "解算选项";
    public const string MenuItemHeader = $"{Title}(_S)";

    [ObservableProperty]
    string _solutionName = string.Empty;

    [ObservableProperty]
    InputOptionsViewModel _baseOptions = new();

    [ObservableProperty]
    InputOptionsViewModel _roverOptions = new();

    [ObservableProperty]
    [RegularExpression("^(?:[\\w]\\:|\\\\)(\\\\[\\w]+)+\\.[m|M][n|N][e|E][d|D]$", ErrorMessage = "非法路径")]
    [NotifyDataErrorInfo]
    string _outputFilePath = string.Empty;

    public static Array Parities { get; } = Enum.GetValues(typeof(Parity));

    public static Array InputFormats { get; } = Enum.GetValues(typeof(InputFormat));

    public static int[] DataBits { get; } = [5, 6, 7, 8];

    public static IEnumerable<int> BaudRates { get; } = [2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800, 921600];

    [RelayCommand]
    void SelectOuputFilePath()
    {
        var dialog = new SaveFileDialog()
        {
            Title = "输出历元数据文件至",
            Filter = "MiraiNavi 历元数据文件|*.mned|所有文件|*.*",
            FileName = $"Solution{UtcTime.Now:yyMMddHHmmss}.mned",
            RestoreDirectory = true,
            CheckPathExists = true,
        };
        if (dialog.ShowDialog() == true)
            OutputFilePath = dialog.FileName;
    }

    public new bool HasErrors => base.HasErrors || BaseOptions.HasErrors || RoverOptions.HasErrors;

    public bool HasNotErrors => !HasErrors;

    public SolutionOptions GetSolutionOptions()
    {
        return new SolutionOptions()
        {
            Name = SolutionName,
            BaseOptions = BaseOptions.GetInputOptions(),
            RoverOptions = RoverOptions.GetInputOptions(),
            OutputFilePath = OutputFilePath,
        };
    }

    public bool TryGetSolutionOptions([NotNullWhen(true)] out SolutionOptions? options)
    {
        if (HasErrors)
        {
            options = default;
            return false;
        }
        options = GetSolutionOptions();
        return true;
    }

    void SetProperties(SolutionOptions options)
    {
        SolutionName = options.Name;
        BaseOptions = new InputOptionsViewModel(options.BaseOptions);
        RoverOptions = new InputOptionsViewModel(options.RoverOptions);
        OutputFilePath = options.OutputFilePath;
    }

    [RelayCommand]
    void ReadSolutionOptions()
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
            var options = JsonSerializer.Deserialize<SolutionOptions>(content, _jsonSerializerOptions);
            if (options is null)
                return;
            SetProperties(options);
        }
        catch (Exception)
        {
            // TODO 异常处理
        }
    }

    [RelayCommand(CanExecute = nameof(HasNotErrors))]
    void SaveSolutionOptions()
    {
        var solutionName = SolutionName == string.Empty ? "未命名预设" : SolutionName;
        var builder = new StringBuilder(solutionName);
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
        var content = JsonSerializer.Serialize(GetSolutionOptions(), _jsonSerializerOptions);
        File.WriteAllText(dialog.FileName, content, Encoding.UTF8);
    }

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
}
