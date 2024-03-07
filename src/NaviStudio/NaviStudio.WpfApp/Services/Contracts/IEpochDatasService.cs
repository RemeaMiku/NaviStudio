namespace NaviStudio.WpfApp.Services.Contracts;

public interface IEpochDatasService
{
    #region Public Properties

    public IEnumerable<EpochData> Datas { get; }

    public int EpochCount { get; }

    public bool HasData => EpochCount > 0;

    public EpochData Last { get; }

    #endregion Public Properties

    #region Public Methods

    public void Add(EpochData epochData);

    public EpochData GetByTimeStamp(UtcTime timeStamp);

    public EpochData GetByIndex(int index);

    public void Clear();

    public void Save(string filePath);

    public void Load(string filePath);

    public void StartAutoSave(string filePath);

    public void StopAutoSave();

    #endregion Public Methods
}
