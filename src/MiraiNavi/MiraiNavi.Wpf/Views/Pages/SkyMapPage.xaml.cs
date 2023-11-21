using System.Windows.Controls;
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
        ViewModel = viewModel;
    }

    public SkyMapPageViewModel ViewModel { get; }

}
