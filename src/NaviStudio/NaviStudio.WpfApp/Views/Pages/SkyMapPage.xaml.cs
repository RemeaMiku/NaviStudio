using System.Windows.Controls;
using NaviStudio.Shared.Models.Satellites;
using NaviStudio.WpfApp.ViewModels.Pages;

namespace NaviStudio.WpfApp.Views.Pages;

/// <summary>
/// SkyMapPage.xaml 的交互逻辑
/// </summary>
public partial class SkyMapPage : UserControl
{
    #region Public Constructors

    public SkyMapPage(SkyMapPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = this;
        ViewModel = viewModel;
    }

    #endregion Public Constructors

    #region Public Properties

    public static string XBindingPath => nameof(SatelliteSkyPosition.AzimuthDegrees);


    public static string YBindingPath => nameof(SatelliteSkyPosition.ElevationDegrees);

    public SkyMapPageViewModel ViewModel { get; }

    #endregion Public Properties

}
