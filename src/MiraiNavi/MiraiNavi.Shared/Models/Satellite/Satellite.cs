namespace MiraiNavi.Shared.Models;

public readonly record struct Satellite
{
    public Satellite(string prnCode)
    {
        PrnCode = prnCode;
    }

    public string PrnCode { get; init; }

    public SatelliteSystem SystemType => GetSatelliteSystemType(PrnCode);

    public int PrnNumber => int.Parse(PrnCode[1..]);

    public static implicit operator Satellite(string prnCode) => new(prnCode);

    public static SatelliteSystem GetSatelliteSystemType(string prnCode)
    {
        var systemCode = prnCode.ToUpper()[0];
        return systemCode switch
        {
            'G' => SatelliteSystem.GPS,
            'C' => SatelliteSystem.BDS,
            'R' => SatelliteSystem.GLONASS,
            'E' => SatelliteSystem.Galileo,
            _ => SatelliteSystem.Others,
        };
    }

    public override string ToString() => PrnCode;
}