using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MiraiNavi.WpfApp.Common.Converters;

[ValueConversion(typeof(bool), typeof(string))]
public class BooleanToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool b)
            throw new ArgumentException("Value must be a boolean", nameof(value));
        return b ? "是" : "否";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string s)
            throw new ArgumentException("Value must be a string", nameof(value));
        return s == "是";
    }
}
