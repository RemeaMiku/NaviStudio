namespace NaviStudio.Shared.Models.Satellites;

public record class SatelliteTracking
{
    public Satellite Satellite { get; set; }

    public bool IsUsed { get; set; }

    public double Frequency { get; set; }

    public double SignalNoiseRatio { get; set; }

    //public double SignalStrength => 100 / double.Max(SignalNoiseRatio, 1);

    public override string ToString() => $"{Satellite}: {Frequency}, {SignalNoiseRatio:F1}";
}
