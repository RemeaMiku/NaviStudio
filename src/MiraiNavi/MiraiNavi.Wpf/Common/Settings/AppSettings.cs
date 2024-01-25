using System.ComponentModel.DataAnnotations;

namespace MiraiNavi.WpfApp.Common.Settings;

public class AppSettings
{
    #region Public Properties

    public AppearanceSettings AppearanceSettings { get; set; } = new();

    public SolutionSettings SolutionSettings { get; set; } = new();

    #endregion Public Properties

    #region Public Methods

    public bool TryValidate()
    {
        return
            Validator.TryValidateObject(AppearanceSettings, new(AppearanceSettings), default) &&
            Validator.TryValidateObject(SolutionSettings, new(SolutionSettings), default);
    }

    #endregion Public Methods
}
