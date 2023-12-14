using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IGMapRouteDisplayService
{
    public IGMapRouteDisplayService RegisterGMapControl(GMapControl gMapControl);

    public IGMapRouteDisplayService RegisterPositionMarker(GMapMarker positionMarker);

    public PointLatLng Position { get; }

    public void AddPoint(PointLatLng point, UtcTime timeStamp, bool updatePositionMarker = false);

    public void AddPoints(IEnumerable<(PointLatLng point, UtcTime timeStamp)> points, bool updatePositionMarker = false);

    public void ClearPoints();

    public void MoveTo(int index);

    public void MoveToOffset(int offset);

    public Task StartAsync(double timeScale = 1);

    public void Pause();

    public void Stop();
}
