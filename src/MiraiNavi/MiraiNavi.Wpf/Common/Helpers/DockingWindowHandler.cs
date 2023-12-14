using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MiraiNavi.WpfApp.Common.Helpers;

public static class DockingWindowHandler
{
    public static void SetViewModelIsActive(ContentControl contentControl, bool isActive)
    {
        var viewModel = contentControl.Content?.GetType()?.GetProperty("ViewModel")?.GetValue(contentControl.Content);
        viewModel?.GetType()?.GetProperty("IsActive")?.SetValue(viewModel, isActive);
    }
}
