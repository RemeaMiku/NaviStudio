using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MiraiNavi.WpfApp.Common.Converters;

[ValueConversion(typeof(Enum), typeof(bool))]
public class EnumToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Enum valueEnum)
            throw new ArgumentException("Value must be an enum", nameof(value));
        if (parameter is not Enum paramEnum)
            throw new ArgumentException("Parameter must be an enum", nameof(parameter));
        return valueEnum.Equals(paramEnum) ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
