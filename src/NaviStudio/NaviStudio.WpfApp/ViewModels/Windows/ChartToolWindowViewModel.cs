﻿using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NaviStudio.Shared.Models.Chart;

namespace NaviStudio.WpfApp.ViewModels.Windows;

public partial class ChartToolWindowViewModel : ObservableValidator
{
    #region Public Fields

    public const string Title = "创建图表组";
    public const string MenuItemHeader = $"{Title}(_C)";

    #endregion Public Fields

    #region Public Properties

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
        ChartItems.SatelliteCount,
        ChartItems.SatelliteCountOfEachSystem,
    ];

    //public static List<string> SatelliteInfoItems { get; } =
    //[
    //    ChartItems.GPSSatelliteVisibility,
    //    ChartItems.BeidouSatelliteVisibility,
    //    ChartItems.GLONASSSatelliteVisibility,
    //    ChartItems.GalileoSatelliteVisibility,
    //];

    public HashSet<string> SelectedItems { get; } = [];

    public bool CanCreateChartGroup => SelectedItems.Count != 0 && !HasErrors;

    #endregion Public Properties

    #region Private Fields

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(CanCreateChartGroup))]
    [Range(MinEpochCount, MaxEpochCount, ErrorMessage = $"大小需在 1 到 100 之间")]
    int _epochCount = 10;

    public const double MinEpochCount = 1;

    public const double MaxEpochCount = 100000;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [NotifyPropertyChangedFor(nameof(CanCreateChartGroup))]
    [Required(ErrorMessage = "不能为空")]
    string _chartGroupName = DateTime.Now.ToString("HH:mm:ss");

    #endregion Private Fields

    #region Private Methods

    [RelayCommand]
    void SetEpochCountToMax()
    {
        EpochCount = (int)MaxEpochCount;
    }

    [RelayCommand]
    void SelectItem(string item)
    {
        if(!SelectedItems.Add(item))
            SelectedItems.Remove(item);
        OnPropertyChanged(nameof(CanCreateChartGroup));
    }

    #endregion Private Methods
}