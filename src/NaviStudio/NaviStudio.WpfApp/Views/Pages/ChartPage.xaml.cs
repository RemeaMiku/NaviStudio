using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using NaviStudio.Shared.Models.Chart;
using NaviStudio.WpfApp.ViewModels.Pages;
using Syncfusion.UI.Xaml.Charts;

namespace NaviStudio.WpfApp.Views.Pages;

/// <summary>
/// ChartPage.xaml 的交互逻辑
/// </summary>
public partial class ChartPage : UserControl
{
    #region Public Constructors

    public ChartPage(ChartPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = this;
    }

    #endregion Public Constructors

    #region Public Properties

    public ChartPageViewModel ViewModel { get; }

    #endregion Public Properties

    #region Public Methods

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

    #endregion Public Methods

    #region Private Fields

    static readonly Brush[] _brushes =
    [
        (Brush)App.Current.Resources["MikuGreenBrush"],
        (Brush)App.Current.Resources["MeaYellowBrush"],
        (Brush)App.Current.Resources["MikuRedBrush"],
        (Brush)App.Current.Resources["MeaBlueBrush"],
        (Brush)App.Current.Resources["RinOrangeBrush"],
    ];

    #endregion Private Fields

    #region Private Methods

    private void OnSfChartMouseEnter(object sender, MouseEventArgs e)
    {
        ChartZoomPanBehavior.EnableZoomingToolBar = true;
    }

    private void OnSfChartMouseLeave(object sender, MouseEventArgs e)
    {
        ChartZoomPanBehavior.EnableZoomingToolBar = false;
    }

    #endregion Private Methods

    private void OnSaveButtonClicked(object sender, System.Windows.RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg,*.jpeg)|*.jpg;*.jpeg|Gif (*.gif)|*.gif|PNG(*.png)|*.png|TIFF(*.tif,*.tiff)|*.tif|All files (*.*)|*.*"
        };
        if(dialog.ShowDialog() == true)
        {
            using Stream fs = dialog.OpenFile();
            SfChart.Save(fs, new PngBitmapEncoder());
        }
    }
}
