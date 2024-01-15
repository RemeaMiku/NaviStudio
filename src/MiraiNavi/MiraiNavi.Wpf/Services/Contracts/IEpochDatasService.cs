﻿namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IEpochDatasService
{
    public IEnumerable<EpochData> Datas { get; }

    public int EpochCount { get; }

    public bool HasData => EpochCount > 0;

    public void Add(EpochData epochData);

    public EpochData? GetByTimeStamp(UtcTime timeStamp);

    public void Clear();

    public EpochData Last { get; }
}
