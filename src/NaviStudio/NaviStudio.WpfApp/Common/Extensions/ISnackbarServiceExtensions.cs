using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Ui.Common;
using Wpf.Ui.Mvvm.Contracts;

namespace NaviStudio.WpfApp.Common.Extensions;

public static class ISnackbarServiceExtensions
{
    public static bool ShowSuccess(this ISnackbarService snackbarService, string title, string message)
        => snackbarService.Show(title, message, SymbolRegular.CheckmarkCircle24, ControlAppearance.Success);

    public static bool ShowError(this ISnackbarService snackbarService, string title, string message)
        => snackbarService.Show(title, message, SymbolRegular.ErrorCircle24, ControlAppearance.Danger);
}
