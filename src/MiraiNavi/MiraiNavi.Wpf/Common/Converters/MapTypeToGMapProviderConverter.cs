using System.Globalization;
using System.Windows.Data;
using GMap.NET.MapProviders;
using MiraiNavi.Shared.Models.Map;

namespace MiraiNavi.WpfApp.Common.Converters;

[ValueConversion(typeof(MapType), typeof(GMapProvider))]
public class MapTypeToGMapProviderConverter : IValueConverter
{
    #region Public Methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (MapType)value switch
        {
            MapType.Satellite => BingSatelliteMapProvider.Instance,
            MapType.Traffic => OpenCycleTransportMapProvider.Instance,
            MapType.Topographic => OpenCycleMapProvider.Instance,
            MapType.None => GMapProviders.EmptyProvider,
            _ => throw new NotImplementedException(),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion Public Methods
}
