using System.Globalization;
using System.Windows.Data;

namespace NaviStudio.WpfApp.Common.Converters;

[ValueConversion(typeof(bool), typeof(string))]
public class BooleanToStringConverter : IValueConverter
{
    #region Public Methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is not bool b ? throw new ArgumentException("Value must be a boolean", nameof(value)) : (object)(b ? "是" : "否");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is not string s ? throw new ArgumentException("Value must be a string", nameof(value)) : (object)(s == "是");
    }

    #endregion Public Methods
}
