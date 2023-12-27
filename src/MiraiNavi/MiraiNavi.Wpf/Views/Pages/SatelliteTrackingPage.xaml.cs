using System.Windows.Controls;
using MiraiNavi.WpfApp.ViewModels.Pages;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// SatellitePage.xaml 的交互逻辑
/// </summary>
public partial class SatelliteTrackingPage : UserControl
{
    public SatelliteTrackingPage(SatelliteTrackingPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = this;
    }

    public SatelliteTrackingPageViewModel ViewModel { get; }
}
