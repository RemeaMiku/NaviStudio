using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiraiNavi.Shared.Models.Chart;

namespace MiraiNavi.WpfApp.ViewModels.Windows;

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

    public HashSet<string> SelectedItems { get; } = [];

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(CanCreateChartGroup))]
    [Range(3, 100, ErrorMessage = $"大小需在 3 到 100 之间")]
    int _maxEpochCount = 10;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(CanCreateChartGroup))]
    [Required(ErrorMessage = "不能为空")]
    [Length(1, 10, ErrorMessage = "长度需在 1 到 10 之间")]
    string _chartGroupName = "未命名";

    [RelayCommand]
    void SelectItem(string item)
    {
        if (!SelectedItems.Add(item))
            SelectedItems.Remove(item);
        OnPropertyChanged(nameof(CanCreateChartGroup));
    }

    public bool CanCreateChartGroup => SelectedItems.Count != 0 && !HasErrors;
}