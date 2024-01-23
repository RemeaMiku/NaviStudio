using System.Diagnostics;
using System.Windows;
using MiraiNavi.WpfApp.ViewModels.Windows;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Views.Windows;

/// <summary>
/// AppSettingsWindow.xaml 的交互逻辑
/// </summary>
public partial class AppSettingsWindow : UiWindow
{
    public AppSettingsWindow(AppSettingsWindowViewModel viewModel)
    {
        InitializeComponent();
        App.Current.SettingsManager.TryApplyAcrylicIfIsEnabled(this);
        ViewModel = viewModel;
        DataContext = this;
    }

    public AppSettingsWindowViewModel ViewModel { get; }

    private void OnButtonClicked(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
