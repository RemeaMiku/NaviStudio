﻿using System.Globalization;
using System.Windows.Data;
using GMap.NET.MapProviders;

namespace MiraiNavi.WpfApp.Common.Converters;

public class StringToGMapProviderConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (string)value switch
        {
            "卫星" => BingSatelliteMapProvider.Instance,
            "交通" => OpenCycleTransportMapProvider.Instance,
            "地形" => OpenCycleMapProvider.Instance,
            _ => GMapProviders.EmptyProvider,
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
