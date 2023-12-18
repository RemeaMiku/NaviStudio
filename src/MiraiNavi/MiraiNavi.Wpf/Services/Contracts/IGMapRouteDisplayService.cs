using System.Threading;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IGMapRouteDisplayService
{
    public IGMapRouteDisplayService RegisterGMapControl(GMapControl gMapControl);

    public IGMapRouteDisplayService RegisterPositionMarker(GMapMarker positionMarker);

    public PointLatLng? CurrentPosition { get; }

    public void AddPoint(PointLatLng point, UtcTime timeStamp, bool updatePositionMarker = false);

    public void AddPoints(IEnumerable<(PointLatLng point, UtcTime timeStamp)> points, bool updatePositionMarker = false);

    public void Clear();

    public void MoveTo(int index);

    public void MoveToOffset(int offset);

    public Task StartAsync(CancellationToken token, double timeScale = 1);
}
