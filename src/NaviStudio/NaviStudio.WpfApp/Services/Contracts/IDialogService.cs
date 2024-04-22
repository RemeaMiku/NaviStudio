using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NaviStudio.WpfApp.Services.Contracts;

public interface IDialogService
{
    void RegisteDialog(ContentControl control);

    Task<bool> ShowAsync(string? title = default, bool isConfirmRequired = true, bool autoHide = true);

    Task<bool> ShowMessageAsync(string message, string? title = default, bool isConfirmRequired = true, bool autoHide = true);

    void Hide();
}
