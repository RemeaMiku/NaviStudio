using System.Collections.ObjectModel;
using MiraiNavi.Shared.Models.Solution;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.Services;

public class EpochDatasService() : IEpochDatasService
{
    private readonly List<EpochData> _epochDatas = [];

    private readonly HashSet<UtcTime> _existTimeStamps = [];

    public ReadOnlyCollection<EpochData> Datas => _epochDatas.AsReadOnly();

    public void Clear()
    {
        if (_epochDatas.Count == 0)
            return;
        _epochDatas.Clear();
        _existTimeStamps.Clear();
    }

    public void Add(EpochData epochData)
    {
        if (_existTimeStamps.Contains(epochData.TimeStamp))
            throw new ArgumentException("Data for this timestamp already exists.");
        _existTimeStamps.Add(epochData.TimeStamp);
        _epochDatas.Add(epochData);
    }
}
