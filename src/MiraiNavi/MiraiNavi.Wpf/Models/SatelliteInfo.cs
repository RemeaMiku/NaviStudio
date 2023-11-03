namespace MiraiNavi.WpfApp.Models;

public record struct SatelliteInfo
{
    public SatelliteInfo(string prnCode)
    {
        PrnCode = prnCode;
    }
    public string PrnCode { get; set; }

    public static SatelliteInfo Empty { get; } = new(string.Empty);

    public static implicit operator SatelliteInfo(string prnCode) => new(prnCode);

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
