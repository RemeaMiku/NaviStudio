using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Common.Settings;

public record AppearanceSettings
{
    public bool EnableAcrylic { get; set; } = true;
}
