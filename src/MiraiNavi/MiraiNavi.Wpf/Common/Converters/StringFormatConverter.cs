using System.Globalization;
using System.Windows.Data;

namespace MiraiNavi.WpfApp.Common.Converters;

[ValueConversion(typeof(string), typeof(string))]
public class StringFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => string.Format((string)parameter, value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value;
}
