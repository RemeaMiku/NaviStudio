using System.Threading;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using NaviStudio.Shared.Models.Map;

namespace NaviStudio.WpfApp.Services.Contracts;

public interface IGMapRouteDisplayService
{
    public event EventHandler? CurrentPositionChanged;

    #region Public Properties

    public PointLatLng? CurrentPosition { get; }

    public int CurrentPositionIndex { get; }

    public NavigationIndicators Indicator { get; set; }

    public bool EnableMapBearing { get; set; }

    public bool KeepCenter { get; set; }

    public double TimeScale { get; set; }

    #endregion Public Properties

    #region Public Methods

    public IGMapRouteDisplayService RegisterGMapControl(GMapControl gMapControl);

    public void AddPoint(MapPoint point, bool moveToLast = false);

    public void AddPoints(IEnumerable<MapPoint> points, bool moveToLast = false);

    public void Clear();

    public void MoveTo(int index);

    public void MoveToOffset(int offset);

    public Task StartAsync(CancellationToken token);

    #endregion Public Methods
}
