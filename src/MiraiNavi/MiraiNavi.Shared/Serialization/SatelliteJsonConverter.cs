using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MiraiNavi.Shared.Models.Satellites;

namespace MiraiNavi.Shared.Serialization;

public class SatelliteJsonConverter : JsonConverter<Satellite>
{
    public override Satellite Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetString()!;

    public override void Write(Utf8JsonWriter writer, Satellite value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.PrnCode);

}
