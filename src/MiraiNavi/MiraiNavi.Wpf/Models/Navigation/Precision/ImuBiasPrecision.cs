﻿using System.Numerics;

namespace MiraiNavi.WpfApp.Models.Navigation.Precision;

public record class ImuBiasPrecision
{
    public UtcTime? TimeStamp { get; init; }

    public Vector3 StdAccelerometerBias { get; init; }

    public Vector3 StdGyroscopeBias { get; init; }
}
