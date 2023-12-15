using MiraiNavi.WpfApp.Models.Navigation;
using MiraiNavi.WpfApp.Models.Navigation.Precision;

namespace MiraiNavi.WpfApp.Models;

public record class EpochData
{
    public UtcTime TimeStamp { get; init; }

    public Pose? Pose { get; init; }

    public double? EastBaseLine { get; init; }

    public double? NorthBaseLine { get; init; }

    public double? UpBaseLine { get; init; }

    public ImuBias? ImuBias { get; init; }

    public PosePrecision? PosePrecision { get; init; }

    public BaseLinePrecision? BaseLinePrecision { get; init; }

    public ImuBiasPrecision? ImuBiasPrecision { get; init; }
}
