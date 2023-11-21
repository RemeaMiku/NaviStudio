namespace MiraiNavi.WpfApp.Models;

public readonly record struct Satellite
{
    public Satellite(string prnCode)
    {
        PrnCode = prnCode;
    }

    public string PrnCode { get; init; }

    public SatelliteSystemType SystemType => GetSatelliteSystemType(PrnCode);

    public int PrnNumber => int.Parse(PrnCode[1..]);

    public static implicit operator Satellite(string prnCode) => new(prnCode);

    public static SatelliteSystemType GetSatelliteSystemType(string prnCode)
    {
        var systemCode = prnCode.ToUpper()[0];
        return systemCode switch
        {
            'G' => SatelliteSystemType.GPS,
            'C' => SatelliteSystemType.BDS,
            'R' => SatelliteSystemType.GLONASS,
            'E' => SatelliteSystemType.Galileo,
            _ => SatelliteSystemType.Others,
        };
    }

    public override string ToString() => PrnCode;

    //public static char GetSatelliteSystemCode(SatelliteSystemType satelliteSystemType)
    //{
    //    switch (satelliteSystemType)
    //    {
    //        case SatelliteSystemType.Gps:
    //            return 'G';
    //        case SatelliteSystemType.Bds:
    //            return 'C';
    //        case SatelliteSystemType.Glonass:
    //            return ''
    //        case SatelliteSystemType.Galileo:
    //            break;
    //        default:
    //            break;
    //    }
    //}
}