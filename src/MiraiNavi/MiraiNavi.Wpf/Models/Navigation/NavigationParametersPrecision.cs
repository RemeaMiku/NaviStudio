using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaviSharp;

namespace MiraiNavi.WpfApp.Models.Navigation;

public record class NavigationParametersPrecision
{
    public UtcTime? TimeStamp { get; init; }

    public float StdEastVelocity { get; init; }

    public float StdNorthVelocity { get; init; }

    public float StdUpVelocity { get; init; }

    public float StdEastNorthVelocity { get; init; }

    public float StdEastUpVelocity { get; init; }

    public float StdNorthUpVelocity { get; init; }

    public Angle StdYaw { get; init; }

    public Angle StdPitch { get; init; }

    public Angle StdRoll { get; init; }

    public Angle StdYawPitch { get; init; }

    public Angle StdYawRoll { get; init; }

    public Angle StdPitchRoll { get; init; }
}
