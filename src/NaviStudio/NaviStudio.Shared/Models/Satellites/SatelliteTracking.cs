namespace NaviStudio.Shared.Models.Satellites;

public record struct SatelliteTracking
{
    public Satellite Satellite { get; set; }

    public bool IsUsed { get; set; }

    public double Frequency { get; set; }

    public double SignalNoiseRatio { get; set; }

    public override readonly string ToString() => $"{Satellite}: {Frequency}, {SignalNoiseRatio:F1}";
}
