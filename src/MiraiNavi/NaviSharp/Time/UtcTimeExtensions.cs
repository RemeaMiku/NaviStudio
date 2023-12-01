// RemeaMiku(Wuhan University)
//  Email:2020302142257@whu.edu.cn

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviSharp.Time;

public static class UtcTimeExtensions
{
    public static GpsTime ToGpsTime(this UtcTime utcTime) => GpsTime.FromUtc(utcTime);

}
