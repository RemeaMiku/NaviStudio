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
        using var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);
        using var reader = new StreamReader(stream);
        try
        {
            settings = JsonSerializer.Deserialize<AppSettings>(reader.ReadToEnd(), _serializerOptions);
            if(settings is not null && settings.TryValidate())
                Settings = settings;
            return Settings;
        }
        catch(JsonException)
        {
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
        using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(stream);
        writer.Write(JsonSerializer.Serialize(Settings, _serializerOptions));
    }

    public bool TryApplyAcrylicIfIsEnabled(UiWindow window, bool autoSave = true)
    {
        if(!Settings.AppearanceSettings.EnableAcrylic)
        {
            window.WindowBackdropType = BackgroundType.None;
            return false;
        }
        try
        {
            window.WindowBackdropType = BackgroundType.Acrylic;
            return true;
        }
        catch(Exception)
        {
            window.WindowBackdropType = BackgroundType.None;
            Settings.AppearanceSettings.EnableAcrylic = false;
            if(autoSave)
            {
                if(FilePath is null)
                    throw new InvalidOperationException("Settings have not been loaded.");
                Save();
            }
            return false;
        }
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
