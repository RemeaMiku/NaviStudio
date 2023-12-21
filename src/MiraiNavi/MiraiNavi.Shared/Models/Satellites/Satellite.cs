using System.Text.Json.Serialization;
using MiraiNavi.Shared.Serialization;

namespace MiraiNavi.Shared.Models.Satellites;

[JsonConverter(typeof(SatelliteJsonConverter))]
public record struct Satellite
{
    public Satellite(string prnCode)
    {
        if (prnCode.Length != 3)
            throw new ArgumentException("PRN code must be 3 characters long.");
        PrnCode = prnCode;
    }

    public string PrnCode { get; set; }

    public readonly SatelliteSystems System => (SatelliteSystems)PrnCode[0];

    public readonly int Number => int.Parse(PrnCode[1..]);

    public static implicit operator Satellite(string prnCode) => new(prnCode);

    public override readonly string ToString() => PrnCode;
}