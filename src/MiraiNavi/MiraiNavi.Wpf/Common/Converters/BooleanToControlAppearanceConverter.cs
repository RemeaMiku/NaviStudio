using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Common;

namespace MiraiNavi.WpfApp.Common.Converters;

[ValueConversion(typeof(bool), typeof(ControlAppearance))]
public class BooleanToControlAppearanceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool flag)
            return flag ? (ControlAppearance)parameter : ControlAppearance.Secondary;
        throw new ArgumentException("value is not bool");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}