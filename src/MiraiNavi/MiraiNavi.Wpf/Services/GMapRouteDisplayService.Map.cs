using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using MiraiNavi.Shared.Models.Map;
using System.Windows.Controls;
using GMap.NET;

namespace MiraiNavi.WpfApp.Services;

partial class GMapRouteDisplayService
{
    #region Public Properties

    public NavigationIndicators Indicator
    {
        get => _indicator;
        set
        {
            if (_indicator == value)
                return;
            _indicator = value;
            SetPositionMarkerShape();
        }
    }

    public bool EnableMapBearing
    {
        get => _enableMapBearing;
        set
        {
            if (_enableMapBearing == value)
                return;
            _enableMapBearing = value;
            if (value)
            {
                _positionMarkerRotateTransform.Angle = 0;
                SetMapBearing();
            }
            else
            {
                if (_gMapControl is not null)
                    _gMapControl.Bearing = 0;
                SetPosiitonMarkerRotateTransform();
            }
        }
    }

    public bool KeepCenter
    {
        get => _keepCenter;
        set
        {
            if (_keepCenter == value)
                return;
            _keepCenter = value;
            if (value && _gMapControl is not null && CurrentPosition.HasValue)
                _gMapControl.Position = CurrentPosition.Value;
        }
    }

    #endregion Public Properties

    #region Private Fields

    const double _updateBearingMinDistance = 0.0005;

    const string _basePath = "/Assets/Map/";

    const string _carPath = _basePath + "car.png";

    const string _planePath = _basePath + "plane.png";

    const string _ellipsePath = _basePath + "ellipse.png";

    const string _defaultPath = _basePath + "default.png";

    const double _positionMarkerSize = 30;

    readonly static Dictionary<NavigationIndicators, ImageSource> _indicatorImages = new()
    {
        { NavigationIndicators.Default, new BitmapImage(new(_defaultPath, UriKind.Relative)) },
        { NavigationIndicators.Ellipse, new BitmapImage(new(_ellipsePath, UriKind.Relative)) },
        { NavigationIndicators.Car, new BitmapImage(new(_carPath, UriKind.Relative)) },
        { NavigationIndicators.Plane, new BitmapImage(new(_planePath, UriKind.Relative)) },
    };

    readonly RotateTransform _positionMarkerRotateTransform = new()
    {
        CenterX = _positionMarkerSize / 2,
        CenterY = _positionMarkerSize / 2
    };

    NavigationIndicators _indicator = NavigationIndicators.Default;
    bool _enableMapBearing = true;
    bool _keepCenter = true;

    #endregion Private Fields

    #region Private Methods

    void SetMapBearing()
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        if (CurrentPosition is null || _positionIndex == 0)
        {
            _gMapControl.Bearing = 0;
            return;
        }
        var previousPosition = _routeMarkers[_positionIndex - 1].Position;
        if (Projection.GetDistance(previousPosition, CurrentPosition.Value) > _updateBearingMinDistance)
            _gMapControl.Bearing = (float)Projection.GetBearing(previousPosition, CurrentPosition.Value);
    }

    void SetPosiitonMarkerRotateTransform()
    {
        ArgumentNullException.ThrowIfNull(_positionMarker);
        if (CurrentPosition is null || _positionIndex == 0)
        {
            _positionMarkerRotateTransform.Angle = 0;
            return;
        }
        var previousPosition = _routeMarkers[_positionIndex - 1].Position;
        if (Projection.GetDistance(previousPosition, CurrentPosition.Value) > _updateBearingMinDistance)
            _positionMarkerRotateTransform.Angle = (float)Projection.GetBearing(previousPosition, CurrentPosition.Value);
    }
    void SetPositionMarkerShape()
    {
        ArgumentNullException.ThrowIfNull(_positionMarker);
        var shape = new Image()
        {
            Width = _positionMarkerSize,
            Height = _positionMarkerSize,
            Source = _indicatorImages[_indicator],
            Effect = new DropShadowEffect()
            {
                BlurRadius = 5,
                Color = Colors.Black,
                Opacity = 0.831,
                ShadowDepth = 0,
            },
            RenderTransform = _positionMarkerRotateTransform
        };
        _positionMarker.Shape = shape;
        _positionMarker.Offset = new(-shape.Width / 2, -shape.Height / 2);
    }

    void UpdatePositionMarker(PointLatLng point)
    {
        ArgumentNullException.ThrowIfNull(_gMapControl);
        _positionMarker.Position = point;
        _positionIndex = _routeMarkers.Count - 1;
        if (EnableMapBearing)
            SetMapBearing();
        else
            SetPosiitonMarkerRotateTransform();
        if (KeepCenter)
            _gMapControl.Position = point;
    }

    #endregion Private Methods
}
