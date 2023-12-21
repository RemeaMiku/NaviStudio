using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NaviSharp.SpatialReference;

namespace MiraiNavi.Shared.Serialization;

public class Vector3JsonConverter : JsonConverter<Vector3>
{
    public override Vector3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();
        var dictionary = new Dictionary<string, float>();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();
            var propertyName = reader.GetString();
            reader.Read();
            if (reader.TokenType != JsonTokenType.Number)
                throw new JsonException();
            dictionary.Add(propertyName!, reader.GetSingle());
        }
        var xPropertyName = options.PropertyNamingPolicy?.ConvertName(nameof(EcefCoord.X)) ?? nameof(EcefCoord.X);
        var yPropertyName = options.PropertyNamingPolicy?.ConvertName(nameof(EcefCoord.Y)) ?? nameof(EcefCoord.Y);
        var zPropertyName = options.PropertyNamingPolicy?.ConvertName(nameof(EcefCoord.Z)) ?? nameof(EcefCoord.Z);
        var x = dictionary[xPropertyName];
        var y = dictionary[yPropertyName];
        var z = dictionary[zPropertyName];
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
