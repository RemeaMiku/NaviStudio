using System.Collections.Frozen;
using MiraiNavi.Shared.Models.Solution;

namespace MiraiNavi.WpfApp.Models;

public partial class ChartParameters
{
    public string Title { get; init; } = default!;    

    public FrozenDictionary<string, Func<EpochData, double?>> LabelFuncs { get; init; } = default!;
}
