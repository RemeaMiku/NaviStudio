using System.Globalization;
using System.Windows.Data;
using MiraiNavi.WpfApp.Models.Output;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Common.Converters;

[ValueConversion(typeof(OutputType), typeof(InfoBarSeverity))]
public class OutputTypeToInfoBarSeverityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            OutputType.Info => InfoBarSeverity.Informational,
            OutputType.Warning => InfoBarSeverity.Warning,
            OutputType.Error => InfoBarSeverity.Error,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
