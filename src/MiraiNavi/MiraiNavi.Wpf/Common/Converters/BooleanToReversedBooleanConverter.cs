using System.Globalization;
using System.Windows.Data;

namespace MiraiNavi.WpfApp.Common.Converters;

[ValueConversion(typeof(bool), typeof(bool))]
public class BooleanToReversedBooleanConverter : IValueConverter
{
    #region Public Methods

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

    #endregion Public Methods
}
