using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using NaviStudio.Shared.Serialization;
using NaviStudio.WpfApp.Services.Contracts;

namespace NaviStudio.WpfApp.Services;

public class JsonEpochDatasService : IEpochDatasService
{

    #region Public Properties   

    public IEnumerable<EpochData> Datas => _datas;

    public int EpochCount => _datas.Count;

    public EpochData Last => _datas.Last();

    #endregion Public Properties

    #region Public Methods

    public void Clear()
    {
        if(_datas.Count == 0)
            return;
        _datas.Clear();
    }

    public void Add(EpochData epochData)
    {
        _datas.Add(epochData);
        _autoSaveWriter?.WriteLine(JsonSerializer.Serialize(epochData, _jsonSerializerOptions));
    }

    public EpochData GetByTimeStamp(UtcTime timeStamp)
    {
        var index = _datas.BinarySearch(new EpochData { TimeStamp = timeStamp }, EpochDataTimeStampComparer.Default);
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0, nameof(index));
        return _datas[index];
    }

    public EpochData GetByIndex(int index)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(index, _datas.Count, nameof(index));
        ArgumentOutOfRangeException.ThrowIfLessThan(index, 0, nameof(index));
        return _datas[index];
    }

    readonly static JsonSerializerOptions _jsonSerializerOptions = new()
    {
        IgnoreReadOnlyProperties = true,
        Converters = { new UtcTimeJsonConverter() }
    };

    public void Save(string filePath)
    {
        File.WriteAllLines(filePath, _datas.Select(d => JsonSerializer.Serialize(d, _jsonSerializerOptions)));
    }

    public void Load(string filePath)
    {
        using var reader = new StreamReader(filePath, Encoding.UTF8);
        _datas.Clear();
        while(!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if(string.IsNullOrWhiteSpace(line))
                continue;
            var data = JsonSerializer.Deserialize<EpochData>(line, _jsonSerializerOptions);
            if(data is null)
                continue;
            _datas.Add(data);
        }
        //var datas = JsonSerializer.Deserialize<List<EpochData>>(File.ReadAllText(filePath));
        //if(datas is null)
        //    return;
        //_datas = datas;
    }

    StreamWriter? _autoSaveWriter;

    public void StartAutoSave(string filePath)
    {
        _autoSaveWriter = new(filePath, false, Encoding.UTF8)
        {
            AutoFlush = true
        };
    }

    public void StopAutoSave()
    {
        if(_autoSaveWriter is null)
            return;
        _autoSaveWriter.Dispose();
        _autoSaveWriter = default;
    }

    class EpochDataTimeStampComparer : IComparer<EpochData>
    {
        public static readonly EpochDataTimeStampComparer Default = new();

        public int Compare(EpochData? x, EpochData? y)
        {
            return x is null ? y is null ? 0 : -1 : y is null ? 1 : x.TimeStamp.CompareTo(y.TimeStamp);
        }
    }

    #endregion Public Methods

    #region Private Fields

    readonly List<EpochData> _datas = [];

    #endregion Private Fields
}
