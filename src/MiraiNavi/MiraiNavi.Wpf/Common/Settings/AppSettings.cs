using System.ComponentModel.DataAnnotations;

namespace MiraiNavi.WpfApp.Common.Settings;

public record AppSettings
{
    public AppearanceSettings AppearanceSettings { get; set; } = new();

    public bool TryValidate()
    {
        return Validator.TryValidateObject(AppearanceSettings, new(AppearanceSettings), default);
    }
}
