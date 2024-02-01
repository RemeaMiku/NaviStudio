namespace NaviStudio.Shared.Models.Navi;

public record struct Enu
{
    public double E { get; set; }

    public double N { get; set; }

    public double U { get; set; }

    public Enu(params double[] enu)
    {
        ArgumentOutOfRangeException.ThrowIfNotEqual(enu.Length, 3, nameof(enu));
        E = enu[0];
        N = enu[1];
        U = enu[2];
    }

    public readonly double Length() => Math.Sqrt(E * E + N * N + U * U);

    public override readonly string ToString() => $"E: {E}, N: {N}, U:{U}";
}
