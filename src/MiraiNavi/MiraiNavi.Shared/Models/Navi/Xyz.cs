namespace MiraiNavi.Shared.Models.Navi;

public record struct Xyz
{
    public double X { get; set; }

    public double Y { get; set; }

    public double Z { get; set; }

    public Xyz(params double[] xyz)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(xyz.Length, 3, nameof(xyz));
        X = xyz[0];
        Y = xyz[1];
        Z = xyz[2];
    }

    public readonly double Length() => Math.Sqrt(X * X + Y * Y + Z * Z);

    public override readonly string ToString() => $"X: {X}, Y: {Y}, Z: {Z}";
}
