using System.IO.Ports;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using MiraiNavi.Shared.Models.Solution;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class StartOptionsPageViewModel : ObservableValidator
{
    public const string Title = "开始选项";
    public const string MenuItemHeader = $"{Title}(_S)";

    [ObservableProperty]
    InputOptions _baseInputOptions = InputOptions.DefaultBaseOptions;

    [ObservableProperty]
    InputOptions _roverInputOptions = new();

    public Array Parities { get; } = Enum.GetValues(typeof(Parity));

    public Array StopBits { get; } = Enum.GetValues(typeof(StopBits));

    public Array InputFormats { get; } = Enum.GetValues(typeof(InputFormat));

    public Array InputTypes { get; } = Enum.GetValues(typeof(InputType));
}
