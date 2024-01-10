using System.Collections.ObjectModel;
using System.Linq;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IEpochDatasService
{
    public IEnumerable<EpochData> Datas { get; }

    public int EpochCount { get; }

    public void Add(EpochData epochData);

    public EpochData? GetByTimeStamp(UtcTime timeStamp);

    public void Clear();

    public EpochData? Last { get; }
}
