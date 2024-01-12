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
        var context = new ValidationContext(AppearanceSettings);
        return Validator.TryValidateObject(context, context, default);
    }
}
