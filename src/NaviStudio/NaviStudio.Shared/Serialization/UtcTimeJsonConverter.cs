using System.Text.Json;
using System.Text.Json.Serialization;

namespace NaviStudio.Shared.Serialization;

public class UtcTimeJsonConverter : JsonConverter<UtcTime>
{
    #region Public Methods

    public override UtcTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => UtcTime.ParseExact(reader.GetString()!, "yyyy/MM/dd HH:mm:ss.fff", null);

    public override void Write(Utf8JsonWriter writer, UtcTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString("yyyy/MM/dd HH:mm:ss.fff"));

    #endregion Public Methods

}
