using System.Collections.Frozen;

namespace MiraiNavi.WpfApp.Models;

public partial class ChartParameters
{
    public string Title { get; init; } = default!;

    public FrozenDictionary<string, Func<EpochData, double?>> LabelFuncs { get; init; } = default!;
}
