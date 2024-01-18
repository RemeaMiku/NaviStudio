using System.Diagnostics;
using System.Windows.Controls;
using Microsoft.Win32;
using MiraiNavi.WpfApp.ViewModels.Pages;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// StartPage.xaml 的交互逻辑
/// </summary>
public partial class SolutionOptionsPage : UserControl
{
    public SolutionOptionsPage(SolutionOptionsPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = this;
        ViewModel = viewModel;
    }

    public SolutionOptionsPageViewModel ViewModel { get; }
}
