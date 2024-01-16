using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MiraiNavi.WpfApp.Common.Converters;

[ValueConversion(typeof(IPAddress), typeof(string))]
public class IPAddressToStringConverter : IValueConverter
{
    object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not IPAddress ipAddress)
            throw new ArgumentException("value is not IPAddress", nameof(value));
        return ipAddress.ToString();
    }

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string str)
            throw new ArgumentException("value is not string", nameof(value));
        if (IPAddress.TryParse(str, out var iPAddress))
            return iPAddress;
        return value;
    }
}
