using System.Globalization;
using System.Windows.Data;
using Wpf.Ui.Controls;

namespace NaviStudio.WpfApp.Common.Converters;

[ValueConversion(typeof(SeverityType), typeof(InfoBarSeverity))]
public class SeverityTypeToInfoBarSeverityConverter : IValueConverter
{
    #region Public Methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            SeverityType.Info => InfoBarSeverity.Informational,
            SeverityType.Warning => InfoBarSeverity.Warning,
            SeverityType.Error => InfoBarSeverity.Error,
            SeverityType.Success => InfoBarSeverity.Success,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion Public Methods
}
