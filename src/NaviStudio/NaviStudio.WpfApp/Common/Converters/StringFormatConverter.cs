using System.Globalization;
using System.Windows.Data;

namespace NaviStudio.WpfApp.Common.Converters;

[ValueConversion(typeof(string), typeof(string))]
public class StringFormatConverter : IValueConverter
{
    #region Public Methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => string.Format((string)parameter, value);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => value;

    #endregion Public Methods
}
