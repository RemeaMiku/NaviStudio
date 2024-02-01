using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NaviStudio.Shared.Models.Satellites;

namespace NaviStudio.WpfApp.Common.Converters;

[ValueConversion(typeof(SatelliteSystems), typeof(ImageSource))]
public class SatelliteSystemsToImageSourceConverter : IValueConverter
{
    #region Public Methods

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return _satelliteSystemImages[(SatelliteSystems)value];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    #endregion Public Methods

    #region Private Fields

    const string _basePath = "/Assets/Satellites/";
    const string _gpsPath = _basePath + "gps.png";
    const string _beidouPath = _basePath + "beidou.png";
    const string _glonassPath = _basePath + "glonass.png";
    const string _galileoPath = _basePath + "galileo.png";
    const string _othersPath = _basePath + "others.png";

    readonly static Dictionary<SatelliteSystems, ImageSource> _satelliteSystemImages = new()
    {
        { SatelliteSystems.GPS, new BitmapImage(new(_gpsPath, UriKind.Relative)) },
        { SatelliteSystems.Beidou, new BitmapImage(new(_beidouPath, UriKind.Relative)) },
        { SatelliteSystems.GLONASS, new BitmapImage(new(_glonassPath, UriKind.Relative)) },
        { SatelliteSystems.Galileo, new BitmapImage(new(_galileoPath, UriKind.Relative)) },
        { SatelliteSystems.Others, new BitmapImage(new(_othersPath, UriKind.Relative)) },
    };

    #endregion Private Fields
}
