using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MiraiNavi.WpfApp.Common.Converters;

public class SatelliteSystemTypeToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (SatelliteSystems)value switch
        {
            SatelliteSystems.GPS => Brushes.Navy,
            SatelliteSystems.BDS => Brushes.Red,
            SatelliteSystems.GLONASS => Brushes.Green,
            SatelliteSystems.Galileo => Brushes.Orange,
            SatelliteSystems.Others => Brushes.Purple,
            _ => throw new ArgumentOutOfRangeException(nameof(value)),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
