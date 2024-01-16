using System.ComponentModel.DataAnnotations;

namespace MiraiNavi.WpfApp.Common.Settings;

public class AppSettings
{
    public AppearanceSettings AppearanceSettings { get; set; } = new();

    public SolutionSettings SolutionSettings { get; set; } = new();

    public bool TryValidate()
    {
        return
            Validator.TryValidateObject(AppearanceSettings, new(AppearanceSettings), default) &&
            Validator.TryValidateObject(SolutionSettings, new(SolutionSettings), default);
    }
}
