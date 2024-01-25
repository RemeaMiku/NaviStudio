using GMap.NET;
using NaviSharp.SpatialReference;

namespace MiraiNavi.WpfApp.Common.Extensions;

public static class GeodeticCoordExtensions
{
    #region Public Methods

    public static PointLatLng ToPointLatLng(this GeodeticCoord geodeticCoord)
        => new(geodeticCoord.Latitude.Degrees, geodeticCoord.Longitude.Degrees);

    #endregion Public Methods
}
