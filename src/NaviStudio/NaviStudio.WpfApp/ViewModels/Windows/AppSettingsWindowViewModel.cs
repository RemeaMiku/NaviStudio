using System.ComponentModel.DataAnnotations;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace NaviStudio.WpfApp.ViewModels.Windows;

public partial class AppSettingsWindowViewModel : ObservableValidator
{
    #region Public Fields

    public const string Title = "选项";
    public const string MenuItemHeader = $"{Title}(_O)";

    #endregion Public Fields

    #region Public Constructors

    public AppSettingsWindowViewModel()
    {
        var settings = App.Current.SettingsManager.Settings;
        var appearanceSettings = settings.AppearanceSettings;
        IsAcrylicEnabled = appearanceSettings.EnableAcrylic;
    }

    #endregion Public Constructors

    #region Public Properties

    public bool HasNoErrors => !HasErrors;

    #endregion Public Properties

    #region Private Fields

    [ObservableProperty]
    bool _isAcrylicEnabled;

    #endregion Private Fields

    #region Private Methods

    [RelayCommand(CanExecute = nameof(HasNoErrors))]
    void Save()
    {
        var settings = App.Current.SettingsManager.Settings;
        var appearanceSettings = settings.AppearanceSettings;
        appearanceSettings.EnableAcrylic = IsAcrylicEnabled;
        App.Current.SettingsManager.Save();
        App.Current.TryApplyAcrylicToAllWindowsIfIsEnabled();
    }

    #endregion Private Methods
}
