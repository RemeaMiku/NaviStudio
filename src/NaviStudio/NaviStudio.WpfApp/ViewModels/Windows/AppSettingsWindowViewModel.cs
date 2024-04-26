using System.ComponentModel.DataAnnotations;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NaviStudio.WpfApp.Common.Settings;

namespace NaviStudio.WpfApp.ViewModels.Windows;

public partial class AppSettingsWindowViewModel : ObservableValidator
{
    #region Public Fields

    public const string Title = "选项";
    public const string MenuItemHeader = $"{Title}(_O)";

    #endregion Public Fields

    #region Public Properties

    public bool HasNoErrors => !HasErrors;

    #endregion Public Properties

    #region Private Fields

    [ObservableProperty]
    bool _isAcrylicEnabled;

    [ObservableProperty]
    int _theme;

    #endregion Private Fields

    #region Private Methods

    [RelayCommand]
    void Save()
    {
    }

    #endregion Private Methods
}
