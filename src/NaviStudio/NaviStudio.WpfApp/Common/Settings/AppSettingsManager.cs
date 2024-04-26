using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace NaviStudio.WpfApp.Common.Settings;

public class AppSettingsManager
{
    #region Public Fields

    public const string DefaultFileName = "appsettings.json";

    #endregion Public Fields

    #region Public Properties

    public AppSettings Settings
    {
        get => _settings;
        private set
        {
            if(value != _settings)
            {
                _preSettings = _settings;
                _settings = value;
            }
        }
    }

    public string FilePath { get; private set; } = string.Empty;

    #endregion Public Properties

    #region Public Methods

    public AppSettings Load(string? filePath = default, AppSettings? fallback = default)
    {
        if(string.IsNullOrEmpty(filePath))
            filePath = DefaultFileName;
        ThrowIfNotJson(filePath);
        fallback ??= new();
        FilePath = filePath;
        AppSettings? settings;
        try
        {
            settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(filePath), _serializerOptions);
            if(settings is not null && settings.TryValidate())
                Settings = settings;
            return Settings;
        }
        catch(JsonException)
        {
            Settings = fallback;
            Save();
            return fallback;
        }
    }

    public AppSettings RollBack()
    {
        Settings = _preSettings;
        return Settings;
    }

    public void Save(string? filePath = default)
    {
        filePath ??= FilePath;
        ArgumentException.ThrowIfNullOrEmpty(filePath);
        ThrowIfNotJson(filePath);
        File.WriteAllText(filePath, JsonSerializer.Serialize(Settings, _serializerOptions));
    }

    #endregion Public Methods

    #region Private Fields

    static readonly JsonSerializerOptions _serializerOptions = new()
    {
        IgnoreReadOnlyProperties = true,
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
    };

    AppSettings _settings = new();
    AppSettings _preSettings = new();

    #endregion Private Fields

    #region Private Methods

    static void ThrowIfNotJson(string filePath)
    {
        if(!Path.GetExtension(filePath).Equals(".json", StringComparison.CurrentCultureIgnoreCase))
            throw new ArgumentException("File path not legal.");
    }

    #endregion Private Methods
}
