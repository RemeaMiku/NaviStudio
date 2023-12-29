using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiraiNavi.WpfApp.Models;

public partial class ChartParameters
{
    public string Title { get; init; } = default!;    

    public FrozenDictionary<string, Func<EpochData, double?>> LabelFuncs { get; init; } = default!;
}
