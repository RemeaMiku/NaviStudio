using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GMap.NET.MapProviders;

namespace MiraiNavi.WpfApp.Common.Converters;

public class StringToGMapProvider : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (string)value switch
        {
            "卫星" => BingSatelliteMapProvider.Instance,
            "交通" => OpenCycleTransportMapProvider.Instance,
            "地形" => OpenCycleMapProvider.Instance,
            _ => throw new NotImplementedException(),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value switch
        {
            BingSatelliteMapProvider => "卫星",
            OpenCycleTransportMapProvider => "交通",
            OpenCycleMapProvider => "地形",
            _ => throw new NotImplementedException(),
        };
    }
}
