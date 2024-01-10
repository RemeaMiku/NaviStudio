using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GMap.NET.WindowsPresentation;

namespace MiraiNavi.WpfApp.Common.Extensions;

public static class GMapMarkerExtensions
{
    public static GMapMarker SetShape(this GMapMarker marker, FrameworkElement shape)
    {
        marker.Shape = shape;
        marker.Offset = new(-shape.ActualWidth / 2, -shape.ActualHeight / 2);
        return marker;
    }
}
