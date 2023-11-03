using System.Windows.Controls;
using MiraiNavi.WpfApp.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// SkyMapPage.xaml 的交互逻辑
/// </summary>
public partial class SkyMapPage : UserControl, IHasViewModel<SkyMapPageViewModel>
{
    public SkyMapPage(SkyMapPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }

    public SkyMapPageViewModel ViewModel { get; }
}
