using System.Windows;
using GMap.NET.WindowsPresentation;

namespace NaviStudio.WpfApp.Common.Extensions;

public static class GMapMarkerExtensions
{
    #region Public Methods

    public static GMapMarker SetShape(this GMapMarker marker, FrameworkElement shape)
    {
        marker.Shape = shape;
        marker.Offset = new(-shape.Width / 2, -shape.Height / 2);
        return marker;
    }

    public static bool IsVisible(this GMapMarker marker, GMapControl map)
    {
        var projection = map.MapProvider.Projection;
        var centerPosition = projection.FromLatLngToPixel(map.CenterPosition, (int)map.Zoom);
        var markerPosition = projection.FromLatLngToPixel(marker.Position, (int)map.Zoom);
        var size = Math.Max(map.ActualWidth, map.ActualHeight);
        return Math.Abs(markerPosition.X - centerPosition.X) <= size / 2 && Math.Abs(markerPosition.Y - centerPosition.Y) <= size / 2;
    }

    #endregion Public Methods
}
