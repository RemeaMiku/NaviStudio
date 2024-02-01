using System.Text.Json;
using System.Text.Json.Serialization;
using NaviStudio.Shared.Models.Satellites;

namespace NaviStudio.Shared.Serialization;

public class SatelliteJsonConverter : JsonConverter<Satellite>
{
    #region Public Methods

    public override Satellite Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString()!;

    public override void Write(Utf8JsonWriter writer, Satellite value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.PrnCode);

    #endregion Public Methods

}
