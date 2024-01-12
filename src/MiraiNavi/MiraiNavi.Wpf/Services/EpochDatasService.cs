using System.Collections.ObjectModel;
using System.Linq;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.Services;

public class EpochDatasService : IEpochDatasService
{

    readonly Dictionary<UtcTime, EpochData> _datas = [];

    public EpochData? LastestData => _datas.Values.LastOrDefault();

    public IEnumerable<EpochData> Datas => _datas.Values;

    public int EpochCount => _datas.Count;

    public EpochData Last => _datas.Values.Last();

    public void Clear()
    {
        if (_datas.Count == 0)
            return;
        _datas.Clear();
    }

    public void Add(EpochData epochData)
        => _datas.Add(epochData.TimeStamp, epochData);

    public EpochData? GetByTimeStamp(UtcTime timeStamp)
        => _datas.GetValueOrDefault(timeStamp);
}
