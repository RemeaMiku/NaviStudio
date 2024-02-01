using System.Windows.Controls;
using NaviStudio.WpfApp.ViewModels.Pages;

namespace NaviStudio.WpfApp.Views.Pages;

/// <summary>
/// SatellitePage.xaml 的交互逻辑
/// </summary>
public partial class SatelliteTrackingPage : UserControl
{
    #region Public Constructors

    public SatelliteTrackingPage(SatelliteTrackingPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = this;
    }

    #endregion Public Constructors

    #region Public Properties

    public SatelliteTrackingPageViewModel ViewModel { get; }

    #endregion Public Properties
}
