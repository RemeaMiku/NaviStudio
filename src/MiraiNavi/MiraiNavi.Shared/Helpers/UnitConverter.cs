namespace MiraiNavi.Shared.Common.Helpers;

public static class UnitConverter
{
    #region Public Methods

    public static double MetersPerSecondToKilometersPerHour(double meterPerSecond)
        => meterPerSecond * 3.6;

    /// <summary>
    /// Converts degrees to radian.
    /// </summary>
    /// <param name="degrees">The value in degrees to convert.</param>
    /// <returns>The value from <paramref name="degrees"/> in radian.</returns>
    public static double DegreesToRadians(double degrees) =>
        degrees * _degreesToRadians;

    /// <summary>
    /// Converts radians to degrees.
    /// </summary>
    /// <param name="radians">The value in radians to convert.</param>
    /// <returns>The value from <paramref name="radians"/> in degrees.</returns>
    public static double RadiansToDegrees(double radians) =>
        radians / _degreesToRadians;

    #endregion Public Methods

    #region Private Fields

    const double _degreesToRadians = Math.PI / 180.0;

    #endregion Private Fields
}
