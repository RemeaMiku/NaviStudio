using System.Windows;
using GMap.NET.WindowsPresentation;

namespace MiraiNavi.WpfApp.Common.Extensions;

public static class GMapMarkerExtensions
{
    #region Public Methods

    public static GMapMarker SetShape(this GMapMarker marker, FrameworkElement shape)
    {
        marker.Shape = shape;
        marker.Offset = new(-shape.Width / 2, -shape.Height / 2);
        return marker;
    }

    #endregion Public Methods
}
