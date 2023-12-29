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
        (Brush)App.Current.Resources["MeaYellowBrush"],
        (Brush)App.Current.Resources["MeaBlueBrush"],
        (Brush)App.Current.Resources["MikuRedBrush"],
    ];

    public void CreateSeries(ChartParameters paras)
    {
        ViewModel.Title = paras.Title;
        var index = 0;
        foreach ((var label, _) in paras.LabelFuncs)
        {
            var seriesData = new ObservableCollection<ChartModel>();
            ViewModel.SeriesDatas.Add(label, seriesData);
            Chart.Series.Add(new FastLineSeries()
            {
                Label = label,
                ItemsSource = seriesData,
                ShowEmptyPoints = false,
                XBindingPath = nameof(ChartModel.TimeStamp),
                YBindingPath = nameof(ChartModel.Value),
                Interior = _brushes[index % _brushes.Length],
            });
            index++;
        }
    }
}
