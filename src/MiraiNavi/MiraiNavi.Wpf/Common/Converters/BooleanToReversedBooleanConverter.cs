using System.Globalization;
using System.Windows.Data;

namespace MiraiNavi.WpfApp.Common.Converters;

public class BooleanToReversedBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var booleanValue = (bool)value;
        return !booleanValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var booleanValue = (bool)value;
        return !booleanValue;
    }
}
