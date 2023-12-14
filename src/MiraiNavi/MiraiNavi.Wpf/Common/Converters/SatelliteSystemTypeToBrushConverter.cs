using System.Collections.Frozen;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MiraiNavi.WpfApp.Models.Satellite;

namespace MiraiNavi.WpfApp.Common.Converters;

public class SatelliteSystemTypeToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (SatelliteSystem)value switch
        {
            SatelliteSystem.GPS => Brushes.Navy,
            SatelliteSystem.BDS => Brushes.Red,
            SatelliteSystem.GLONASS => Brushes.Green,
            SatelliteSystem.Galileo => Brushes.Orange,
            SatelliteSystem.Others => Brushes.Purple,
            _ => throw new ArgumentOutOfRangeException(nameof(value)),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
