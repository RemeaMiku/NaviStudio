using System.ComponentModel.DataAnnotations;
using System.Net;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MiraiNavi.WpfApp.ViewModels.Windows;

public partial class AppSettingsWindowViewModel : ObservableValidator
{
    #region Public Fields

    public const string Title = "设置";
    public const string MenuItemHeader = $"{Title}(_S)";

    #endregion Public Fields

    #region Public Constructors

    public AppSettingsWindowViewModel()
    {
        var settings = App.Current.SettingsManager.Settings;
        var appearanceSettings = settings.AppearanceSettings;
        var solutionSettings = settings.SolutionSettings;
        IsAcrylicEnabled = appearanceSettings.EnableAcrylic;
        SolutionEpochDataTcpAddress = solutionSettings.EpochDataTcpOptions.Address;
        SolutionEpochDataTcpPort = solutionSettings.EpochDataTcpOptions.Port;
    }

    #endregion Public Constructors

    #region Public Properties

    public bool HasNoErrors => !HasErrors;

    #endregion Public Properties

    #region Private Fields

    [ObservableProperty]
    bool _isAcrylicEnabled;

    [ObservableProperty]
    [Required(ErrorMessage = "不能为空")]
    [RegularExpression("^((25[0-5]|(2[0-4]|1\\d|[1-9]|)\\d)\\.?\\b){4}$", ErrorMessage = "非法地址")]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    string _solutionEpochDataTcpAddress;

    [ObservableProperty]
    [Required(ErrorMessage = "不能为空")]
    [Range(0, IPEndPoint.MaxPort, ErrorMessage = "非法端口")]
    [NotifyDataErrorInfo]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    int _solutionEpochDataTcpPort;

    #endregion Private Fields

    #region Private Methods

    [RelayCommand(CanExecute = nameof(HasNoErrors))]
    void Save()
    {
        var settings = App.Current.SettingsManager.Settings;
        var appearanceSettings = settings.AppearanceSettings;
        var solutionSettings = settings.SolutionSettings;
        appearanceSettings.EnableAcrylic = IsAcrylicEnabled;
        solutionSettings.EpochDataTcpOptions.Address = SolutionEpochDataTcpAddress;
        solutionSettings.EpochDataTcpOptions.Port = SolutionEpochDataTcpPort;
        App.Current.SettingsManager.Save();
        App.Current.TryApplyAcrylicToAllWindowsIfIsEnabled();
    }

    #endregion Private Methods
}
