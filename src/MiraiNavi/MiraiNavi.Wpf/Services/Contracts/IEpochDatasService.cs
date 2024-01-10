using System.Collections.ObjectModel;
using MiraiNavi.Shared.Models.Solution;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IEpochDatasService
{
    public ReadOnlyCollection<EpochData> Datas { get; }

    public EpochData? LastestData => Datas.Count == 0 ? default : Datas[^1];

    public void Clear();

    public void Add(EpochData epochData);

    public EpochData GetEpochDataByTimeStamp(UtcTime timeStamp)
    {
        var left = 0;
        var right = Datas.Count;
        while (left < right)
        {
            var middle = left + (right - left) / 2;
            if (Datas[middle].TimeStamp > timeStamp)
                right = middle;
            else if (Datas[middle].TimeStamp < timeStamp)
                left = middle + 1;
            else
                return Datas[middle];
        }
        throw new KeyNotFoundException();
    }
}
