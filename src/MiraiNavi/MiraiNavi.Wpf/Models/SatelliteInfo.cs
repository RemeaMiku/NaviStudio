namespace MiraiNavi.WpfApp.Models;

public readonly record struct SatelliteInfo
{
    public SatelliteInfo(string prnCode)
    {
        PrnCode = prnCode;
        SystemType = GetSatelliteSystemType(prnCode);
    }

    public string PrnCode { get; init; }

    public SatelliteSystemType SystemType { get; }

    public static SatelliteInfo Empty { get; } = new(string.Empty);

    public static implicit operator SatelliteInfo(string prnCode) => new(prnCode);

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
