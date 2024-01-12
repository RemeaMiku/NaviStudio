using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Common.Settings;

public record AppSettings
{
    public AppearanceSettings AppearanceSettings { get; set; } = new();

    public bool TryValidate()
    {
        return Validator.TryValidateObject(AppearanceSettings, new(AppearanceSettings), default);
    }
}
