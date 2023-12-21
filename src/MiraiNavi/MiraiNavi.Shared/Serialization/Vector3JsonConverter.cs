using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MiraiNavi.Shared.Serialization;

public class Vector3JsonConverter : JsonConverter<Vector3>
{
    public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var x = reader.GetSingle();
        var y = reader.GetSingle();
        var z = reader.GetSingle();
        return new(x, y, z);
    }

    public override void Write(Utf8JsonWriter writer, Vector3 value, JsonSerializerOptions options)
    {
        var xPropertyName = options.PropertyNamingPolicy?.ConvertName(nameof(value.X)) ?? nameof(value.X);
        var yPropertyName = options.PropertyNamingPolicy?.ConvertName(nameof(value.Y)) ?? nameof(value.Y);
        var zPropertyName = options.PropertyNamingPolicy?.ConvertName(nameof(value.Z)) ?? nameof(value.Z);
        writer.WriteStartObject();
        writer.WriteNumber(xPropertyName, value.X);
        writer.WriteNumber(yPropertyName, value.Y);
        writer.WriteNumber(zPropertyName, value.Z);
        writer.WriteEndObject();
    }
}
