using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MiraiNavi.WpfApp.Common.Converters;

[ValueConversion(typeof(string), typeof(Brush))]
public class BooleanToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool flag)
            throw new ArgumentException("value is not bool");
        if (parameter is not Brush)
            throw new ArgumentException("parameter is not Brush");
        return flag ? parameter : Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
