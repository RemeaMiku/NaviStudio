using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.Common.Helpers;
using MiraiNavi.WpfApp.ViewModels.Windows;
using MiraiNavi.WpfApp.Views.Pages;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Views.Windows;

/// <summary>
/// ChartToolWindowWindow.xaml 的交互逻辑
/// </summary>
public partial class ChartToolWindow : UiWindow
{
    public ChartToolWindow(ChartToolWindowViewModel viewModel)
    {
        InitializeComponent();
        App.Current.SettingsManager.TryApplyAcrylicIfIsEnabled(this);
        ViewModel = viewModel;
        DataContext = this;
    }

    public ChartToolWindowViewModel ViewModel { get; }

    private void OnConfirmButtonClicked(object sender, System.Windows.RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }
}
