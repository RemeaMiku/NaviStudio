using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;

namespace MiraiNavi.WpfApp.Common.Helpers;

public static class RandomDataGenerator
{
    public static IEnumerable<PointLatLng> GetPointLatLngs(int count)
    {
        var random = new Random();
        for (var i = 0; i < count; i++)
        {
            var lat = random.NextDouble() * 180 - 90;
            var lng = random.NextDouble() * 360 - 180;
            yield return new PointLatLng(lat, lng);
        }
    }
}
