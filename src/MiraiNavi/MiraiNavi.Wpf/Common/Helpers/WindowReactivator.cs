using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Miraisoft.WpfTools.Helpers;

public class WindowReactivator
{

    public static void Register(Window window)
    {
        window.Closing += OnWindowClosing;
    }

    private static void OnWindowClosing(object? sender, CancelEventArgs e)
    {
        e.Cancel = true;
    }

    public static void Reactive(Window window)
    {
        if (window.WindowState == WindowState.Minimized)
            window.WindowState = WindowState.Normal;
        if (!window.IsVisible)
            window.Show();
        window.Activate();
    }
}
