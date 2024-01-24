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
        return booleanValue ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var visibilityValue = (Visibility)value;
        return visibilityValue == Visibility.Collapsed;
    }
}
