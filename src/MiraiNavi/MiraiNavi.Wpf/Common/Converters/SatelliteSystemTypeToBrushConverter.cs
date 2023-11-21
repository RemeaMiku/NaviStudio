using System.Collections.Frozen;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using MiraiNavi.WpfApp.Models;

namespace MiraiNavi.WpfApp.Common.Converters;

public class SatelliteSystemTypeToBrushConverter : IValueConverter
{
    public static FrozenDictionary<SatelliteSystemType, Brush> SatelliteSystemTypeToBrushDictionary { get; } =
        new Dictionary<SatelliteSystemType, Brush>()
        {
            { SatelliteSystemType.GPS, Brushes.Navy },
            { SatelliteSystemType.BDS, Brushes.Red },
            { SatelliteSystemType.GLONASS, Brushes.Green },
            { SatelliteSystemType.Galileo, Brushes.Orange },
            { SatelliteSystemType.Others, Brushes.Purple },
        }.ToFrozenDictionary();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return SatelliteSystemTypeToBrushDictionary[(SatelliteSystemType)value];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
