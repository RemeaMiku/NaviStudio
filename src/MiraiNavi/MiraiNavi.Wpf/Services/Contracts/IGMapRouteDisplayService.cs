using System.Threading;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using MiraiNavi.Shared.Models.Map;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IGMapRouteDisplayService
{
    #region Public Properties

    public PointLatLng? CurrentPosition { get; }

    public NavigationIndicators Indicator { get; set; }

    public bool EnableMapBearing { get; set; }

    public bool KeepCenter { get; set; }

    #endregion Public Properties

    #region Public Methods

    public IGMapRouteDisplayService RegisterGMapControl(GMapControl gMapControl);

    public void AddPoint(PointLatLng point, UtcTime timeStamp, bool updatePositionMarker = false);

    public void AddPoints(IEnumerable<(PointLatLng point, UtcTime timeStamp)> points, bool updatePositionMarker = false);

    public void Clear();

    public void MoveTo(int index);

    public void MoveToOffset(int offset);

    public Task StartAsync(CancellationToken token, double timeScale = 1);

    #endregion Public Methods
}
