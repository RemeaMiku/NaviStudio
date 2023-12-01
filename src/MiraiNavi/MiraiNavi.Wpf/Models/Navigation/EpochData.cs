using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Models.Navigation;

public record class EpochData
{
    public EpochData(UtcTime timeStamp, NavigationParameters navigationParameters, ImuBias imuBias, Vector3? enuBaseline = default)
    {
        TimeStamp = timeStamp;
        NavigationParameters = navigationParameters;
        ImuBias = imuBias;
        EnuBaseLine = enuBaseline;
    }

    public UtcTime TimeStamp { get; init; }

    public NavigationParameters NavigationParameters { get; init; }

    public Vector3? EnuBaseLine { get; init; }

    public ImuBias ImuBias { get; init; }

    public NavigationParametersPrecision? NavigationParametersPrecision { get; init; }

    public BaseLinePrecision? BaseLinePrecision { get; init; }

    public ImuBiasPrecision? ImuBiasPrecision { get; init; }

}
