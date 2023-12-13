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

    public void AddPoint(PointLatLng point, UtcTime timeStamp);

    public void ClearPoints();

    public void MoveTo(int index);

    public void MoveToOffset(int offset);

    public Task StartAsync(double timeScale = 1);

    public void Pause();

    public void Stop();
}
