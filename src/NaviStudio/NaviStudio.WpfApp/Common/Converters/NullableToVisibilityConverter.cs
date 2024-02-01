using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NaviStudio.WpfApp.Common.Converters;

/// <summary>
/// 可空类型对象转换为可见性
/// 如果对象为 null，则返回 Collapsed，否则返回 Visible
/// </summary>
[ValueConversion(typeof(bool), typeof(Visibility))]
public class NullableToVisibilityConverter : IValueConverter
{
    #region Public Methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is null ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion Public Methods
}
