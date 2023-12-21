namespace MiraiNavi.Shared.Models;

public record class BaseLinePrecision
{
    public UtcTime TimeStamp { get; set; }

    public float StdEast { get; set; }

    public float StdNorth { get; set; }

    public float StdUp { get; set; }

    public float StdEastNorth { get; set; }

    public float StdEastUp { get; set; }

    public float StdNorthUp { get; set; }
}
