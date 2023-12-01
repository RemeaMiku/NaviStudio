using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Models.Navigation;

public record class ImuData
{
    public UtcTime? TimeStamp { get; init; }

    public Vector3 Acceleration { get; init; }

    public Vector3 AngularVelocity { get; init; }

    public TimeSpan Duration { get; init; }
}
