using System.Windows;
using NaviStudio.WpfApp.Common.Settings;
using NaviStudio.WpfApp.ViewModels.Windows;
using Wpf.Ui.Controls;

namespace NaviStudio.WpfApp.Views.Windows;

/// <summary>
/// AppSettingsWindow.xaml 的交互逻辑
/// </summary>
public partial class AppSettingsWindow : UiWindow
{
    #region Public Constructors

    public AppSettingsWindow(AppSettingsWindowViewModel viewModel)
    {
        InitializeComponent();
        App.Current.ApplyTheme();
        App.Current.ApplyBackground();
        ViewModel = viewModel;
        DataContext = this;
        SetProperties();
    }

    public void SetProperties()
    {
        var settings = App.Current.SettingsManager.Settings;
        var appearanceSettings = settings.AppearanceSettings;
        ViewModel.IsAcrylicEnabled = appearanceSettings.EnableAcrylic;
        ViewModel.Theme = (int)appearanceSettings.ThemeMode;
    }

    public void SaveToSettings()
    {
        var settings = App.Current.SettingsManager.Settings;
        var appearanceSettings = settings.AppearanceSettings;
        appearanceSettings.EnableAcrylic = ViewModel.IsAcrylicEnabled;
        appearanceSettings.ThemeMode = (ThemeModes)ViewModel.Theme;
    }

    #endregion Public Constructors

    #region Public Properties

    public AppSettingsWindowViewModel ViewModel { get; }

    #endregion Public Properties

    #region Private Methods

    private void OnConfirmButtonClicked(object sender, RoutedEventArgs e)
    {
        SaveToSettings();
        App.Current.SettingsManager.Save();
        App.Current.ApplyTheme();
        App.Current.ApplyBackground();
        Close();
    }

    #endregion Private Methods
}
