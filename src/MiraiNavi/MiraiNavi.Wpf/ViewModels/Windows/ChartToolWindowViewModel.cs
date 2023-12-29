using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.ViewModels.Windows;
//TODO 属性验证器
public partial class ChartToolWindowViewModel : ObservableValidator
{
    public static string Title => "图表工具";

    public static List<string> EstimatedResultItems { get; } =
    [
        ChartItems.LongitudeAndLatitude,
        ChartItems.Altitude,
        ChartItems.EulerAngles,
        ChartItems.AccelerometerBias,
        ChartItems.GyroscopeBias,
    ];

    public static List<string> EstimatedAccuracyItems { get; } =
    [
        ChartItems.StdLocalPosition,
        ChartItems.StdVelocity,
        ChartItems.StdEulerAngles,
        ChartItems.StdAccelerometerBias,
        ChartItems.StdGyroscopeBias,
    ];

    public static List<string> QualityCheckItems { get; } =
    [
        ChartItems.Dop,
        ChartItems.Ratio,
        ChartItems.SatelliteCount,
    ];

    public static List<string> SatelliteInfoItems { get; } =
    [
        "卫星可视性",
    ];

    public ObservableCollection<string> SelectedItems { get; } = [];

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(CreateChartGroupCommand))]
    [Range(1, 100, ErrorMessage = "大小需在 1 到 100 之间")]
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