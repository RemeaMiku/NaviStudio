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
    #region Public Constructors

    public ChartToolWindow(ChartToolWindowViewModel viewModel)
    {
        InitializeComponent();
        App.Current.SettingsManager.TryApplyAcrylicIfIsEnabled(this);
        ViewModel = viewModel;
        DataContext = this;
    }

    #endregion Public Constructors

    #region Public Properties

    public ChartToolWindowViewModel ViewModel { get; }

    #endregion Public Properties

    #region Private Methods

    private void OnConfirmButtonClicked(object sender, System.Windows.RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    #endregion Private Methods
}
