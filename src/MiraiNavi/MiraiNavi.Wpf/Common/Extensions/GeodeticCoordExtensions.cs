using GMap.NET;
using NaviSharp.SpatialReference;

namespace MiraiNavi.WpfApp.Common.Extensions;

public static class GeodeticCoordExtensions
{
    public static PointLatLng ToPointLatLng(this GeodeticCoord geodeticCoord)
        => new(geodeticCoord.Latitude.Degrees, geodeticCoord.Longitude.Degrees);
}
