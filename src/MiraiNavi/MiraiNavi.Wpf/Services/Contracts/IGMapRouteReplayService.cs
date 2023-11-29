using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace MiraiNavi.WpfApp.Services.Contracts;

public interface IGMapRouteReplayService
{
    public PointLatLng CurrentPosition { get; }

    public IGMapRouteReplayService Initiliaze(GMapControl gMapControl, GMapMarker positionMarker, Brush beforeFill, Brush afterFill);

    public void SetRoute(IEnumerable<PointLatLng> points, IEnumerable<UtcTime> timeStamps);

    public Task StartAsync(double timeScale = 1);

    public void Stop();

    public void Pause();

    public void SetPosition(int index);
}
