using System.Windows.Controls;
using MiraiNavi.WpfApp.ViewModels.Pages;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// NavigationParameterPage.xaml 的交互逻辑
/// </summary>
public partial class NavigationParameterPage : UserControl
{
    public NavigationParameterPage(NavigationParameterPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
    }

    public NavigationParameterPageViewModel ViewModel { get; }
}
