using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MiraiNavi.WpfApp.Common.Converters;

[ValueConversion(typeof(bool), typeof(Visibility))]
public class ReversedBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var booleanValue = (bool)value;
        return booleanValue ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var visibilityValue = (System.Windows.Visibility)value;
        return visibilityValue == System.Windows.Visibility.Collapsed;
    }
}
