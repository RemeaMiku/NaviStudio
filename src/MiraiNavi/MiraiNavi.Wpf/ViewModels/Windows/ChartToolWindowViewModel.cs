using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiraiNavi.WpfApp.Models.Chart;

namespace MiraiNavi.WpfApp.ViewModels.Windows;
//TODO 属性验证模板
public partial class ChartToolWindowViewModel : ObservableValidator
{
    public const string Title = "创建图表组";
    public const string MenuItemHeader = $"{Title}(_C)";

    public static List<string> EstimatedResultItems { get; } =
    [
        ChartItems.LongitudeAndLatitude,
        ChartItems.Altitude,
        ChartItems.LocalCoord,
        ChartItems.Velocity,
        ChartItems.Attitude,
        //ChartItems.AccelerometerBias,
        //ChartItems.GyroscopeBias,
    ];

    public static List<string> EstimatedAccuracyItems { get; } =
    [
        ChartItems.StdLocalCoord,
        ChartItems.StdVelocity,
        ChartItems.StdAttitude,
        //ChartItems.StdAccelerometerBias,
        //ChartItems.StdGyroscopeBias,
    ];

    public static List<string> QualityCheckItems { get; } =
    [
        ChartItems.Dop,
        ChartItems.Ratio,
        //ChartItems.VisibleSatelliteCount,
    ];

    public static List<string> SatelliteInfoItems { get; } =
    [
    ];

    public ObservableCollection<string> SelectedItems { get; } = [];

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(CreateChartGroupCommand))]
    [Range(1, 100, ErrorMessage = $"大小需在 1 到 100 之间")]
    int _maxEpochCount = 10;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(CreateChartGroupCommand))]
    [Required(ErrorMessage = "不能为空")]
    [Length(1, 20, ErrorMessage = "长度需在 1 到 20 之间")]
    string? _chartGroupName;

    [RelayCommand]
    void SelectItem(string item)
    {
        if (!SelectedItems.Remove(item))
            SelectedItems.Add(item);
        ValidateAllProperties();
        CreateChartGroupCommand.NotifyCanExecuteChanged();
        ClearErrors();
    }

    public bool CanCreateChartGroup => SelectedItems.Count != 0 && !HasErrors;

    public event EventHandler<ChartGroupParameters>? CreateRequested;

    [RelayCommand(CanExecute = nameof(CanCreateChartGroup))]
    void CreateChartGroup()
    {
        var paras = new ChartGroupParameters(ChartGroupName!, MaxEpochCount, SelectedItems);
        CreateRequested?.Invoke(this, paras);
    }
}