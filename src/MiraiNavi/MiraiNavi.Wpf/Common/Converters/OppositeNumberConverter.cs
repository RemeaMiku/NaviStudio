using System.Globalization;
using System.Windows.Data;

namespace MiraiNavi.WpfApp.Common.Converters;

public class OppositeNumberConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var number = (double)value;
        return -number;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var number = (double)value;
        return -number;
    }
}
