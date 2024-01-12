using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MiraiNavi.WpfApp.Common.Settings;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.Common.Helpers;

public static class AppSettingsManager
{
    public static AppSettings Settings
    {
        get => _settings;
        private set
        {
            if (value != _settings)
            {
                _preSettings = _settings;
                _settings = value;
            }
        }
    }

    public static string FilePath { get; private set; } = string.Empty;

    public static AppSettings Load(string filePath, AppSettings fallback)
    {
        ArgumentException.ThrowIfNullOrEmpty(filePath);
        ArgumentNullException.ThrowIfNull(fallback);
        ThrowIfNotJson(filePath);
        FilePath = filePath;
        Settings = fallback;
        try
        {
            AppSettings? settings;
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new StreamReader(stream);
            settings = JsonSerializer.Deserialize<AppSettings>(reader.ReadToEnd(), _serializerOptions);
            if (settings is not null && settings.TryValidate())
                Settings = settings;
        }
        catch (Exception) { }
        return Settings;
    }

    public static AppSettings RollBack()
    {
        Settings = _preSettings;
        return Settings;
    }

    static void ThrowIfNotJson(string filePath)
    {
        if (!Path.GetExtension(filePath).Equals(".json", StringComparison.CurrentCultureIgnoreCase))
            throw new ArgumentException("File path not legal.");
    }

    public static void Save(string? filePath = default)
    {
        filePath ??= FilePath;
        ArgumentException.ThrowIfNullOrEmpty(filePath);
        ThrowIfNotJson(filePath);
        using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(stream);
        writer.Write(JsonSerializer.Serialize(Settings, _serializerOptions));
    }

    static AppSettings _settings = new();
    static AppSettings _preSettings = new();

    static readonly JsonSerializerOptions _serializerOptions = new()
    {
        IgnoreReadOnlyProperties = true,
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
    };
}
