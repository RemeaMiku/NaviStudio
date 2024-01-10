using System.Windows.Controls;
using MiraiNavi.Shared.Models.Satellites;
using MiraiNavi.WpfApp.ViewModels.Pages;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// SkyMapPage.xaml 的交互逻辑
/// </summary>
public partial class SkyMapPage : UserControl
{
    public SkyMapPage(SkyMapPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = this;
        ViewModel = viewModel;
    }

    public static string XBindingPath => nameof(SatelliteSkyPosition.AzimuthDegrees);


    public static string YBindingPath => nameof(SatelliteSkyPosition.ElevationDegrees);

    public SkyMapPageViewModel ViewModel { get; }

}
