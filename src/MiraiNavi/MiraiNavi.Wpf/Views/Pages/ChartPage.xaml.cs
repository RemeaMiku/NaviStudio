using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Models.Chart;
using MiraiNavi.WpfApp.ViewModels.Pages;
using Syncfusion.UI.Xaml.Charts;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// ChartPage.xaml 的交互逻辑
/// </summary>
public partial class ChartPage : UserControl
{
    public ChartPage(ChartPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = this;
        ViewModel.AddSeriesRequested += OnViewModelAddSeriesRequested;
    }

    public void AddSeries(string label)
    {
        var seriesData = new ObservableCollection<ChartModel>();
        ViewModel.SeriesDatas.Add(label, seriesData);
        SfChart.Series.Add(new FastLineSeries()
        {
            Label = label,
            ItemsSource = seriesData,
            ShowEmptyPoints = false,
            XBindingPath = nameof(ChartModel.TimeStamp),
            YBindingPath = nameof(ChartModel.Value),
            Interior = _brushes[SfChart.Series.Count % _brushes.Length],
        });
    }

    void OnViewModelAddSeriesRequested(object? sender, string e) => AddSeries(e);

    public ChartPageViewModel ViewModel { get; }

    static readonly Brush[] _brushes =
    [
        (Brush)App.Current.Resources["MikuGreenBrush"],
        (Brush)App.Current.Resources["MeaYellowBrush"],
        (Brush)App.Current.Resources["MikuRedBrush"],
        (Brush)App.Current.Resources["MeaBlueBrush"],
    ];

    private void OnSfChartMouseEnter(object sender, MouseEventArgs e)
    {
        ChartZoomPanBehavior.EnableZoomingToolBar = true;
    }

    private void OnSfChartMouseLeave(object sender, MouseEventArgs e)
    {
        ChartZoomPanBehavior.EnableZoomingToolBar = false;
    }
}
