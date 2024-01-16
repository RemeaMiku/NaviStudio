using System.Diagnostics;
using System.Windows.Controls;
using MiraiNavi.WpfApp.ViewModels.Pages;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// StartPage.xaml 的交互逻辑
/// </summary>
public partial class StartOptionsPage : UserControl
{
    public StartOptionsPage(StartOptionsPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = this;
        ViewModel = viewModel;
    }

    public StartOptionsPageViewModel ViewModel { get; }
}
