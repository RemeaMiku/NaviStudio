using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using MiraiNavi.WpfApp.Models;
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
    }

    public ChartPageViewModel ViewModel { get; }

    static readonly Brush[] _brushes =
    [
        (Brush)App.Current.Resources["MikuGreenBrush"],
        (Brush)App.Current.Resources["MeaBlueBrush"],
        (Brush)App.Current.Resources["MikuRedBrush"],
        (Brush)App.Current.Resources["MeaYellowBrush"],
    ];

    public void CreateSeries(ChartParameters paras)
    {
        ViewModel.Title = paras.Title;
        for (int i = 0; i < paras.Labels.Length; i++)
        {
            var seriesData = new ObservableCollection<ChartModel>();
            ViewModel.SeriesDatas.Add(paras.Labels[i], seriesData);
            Chart.Series.Add(new FastLineSeries()
            {
                Label = paras.Labels[i],
                ItemsSource = seriesData,
                ShowEmptyPoints = false,
                XBindingPath = nameof(ChartModel.TimeStamp),
                YBindingPath = nameof(ChartModel.Value),
                Interior = _brushes[i % _brushes.Length],
            });
        }
    }
}
