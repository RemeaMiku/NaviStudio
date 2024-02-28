using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Wpf.Ui.Controls;

namespace NaviStudio.WpfApp.Views.Windows;
/// <summary>
/// ExceptionWindow.xaml 的交互逻辑
/// </summary>
public partial class ExceptionWindow : UiWindow
{
    public ExceptionWindow(string message)
    {
        InitializeComponent();
        DataContext = this;
        Message = message;
    }

    public string Message { get; set; }

    public static bool? Show(string message)
    {
        var window = new ExceptionWindow(message);
        return window.ShowDialog();
    }

    public static bool? Show(Exception exception)
    {
        var stringbuilder = new StringBuilder();
        stringbuilder.AppendLine($"Message: {exception.Message}");
        stringbuilder.AppendLine($"Source: {exception.Source}");
        stringbuilder.AppendLine($"HResult: {exception.HResult}");
        stringbuilder.AppendLine($"StackTrace: ");
        stringbuilder.AppendLine(exception.StackTrace);
        if(exception.InnerException != null)
            stringbuilder.AppendLine($"InnerException: {exception.InnerException.Message}");
        if(exception.Data.Count > 0)
        {
            stringbuilder.AppendLine($"Data: ");
            foreach(var key in exception.Data.Keys)
                stringbuilder.AppendLine($"  {key}: {exception.Data[key]}");
        }
        return Show(stringbuilder.ToString());
    }
}
