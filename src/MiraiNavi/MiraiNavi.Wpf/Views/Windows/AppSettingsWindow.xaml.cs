using System.Windows;
using MiraiNavi.WpfApp.ViewModels.Windows;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Views.Windows;

/// <summary>
/// AppSettingsWindow.xaml 的交互逻辑
/// </summary>
public partial class AppSettingsWindow : UiWindow
{
    #region Public Constructors

    public AppSettingsWindow(AppSettingsWindowViewModel viewModel)
    {
        InitializeComponent();
        App.Current.SettingsManager.TryApplyAcrylicIfIsEnabled(this);
        ViewModel = viewModel;
        DataContext = this;
    }

    #endregion Public Constructors

    #region Public Properties

    public AppSettingsWindowViewModel ViewModel { get; }

    #endregion Public Properties

    #region Private Methods

    private void OnButtonClicked(object sender, RoutedEventArgs e)
    {
        Close();
    }

    #endregion Private Methods
}
