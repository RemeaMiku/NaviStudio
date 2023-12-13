using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using GMap.NET;
using MiraiNavi.WpfApp.Models.Navigation;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class MapPageViewModel(IGMapRouteDisplayService gMapRouteReplayService) : ObservableRecipient, IRecipient<EpochData>
{
    readonly IGMapRouteDisplayService _gMapRouteDisplayService = gMapRouteReplayService;

    [ObservableProperty]
    PointLatLng _mapCenter;

    protected override void OnActivated()
    {
        base.OnActivated();
    }

    public void Receive(EpochData message)
    {
        throw new NotImplementedException();
    }

    [RelayCommand]
    void Replay()
    {
        var points = new List<PointLatLng>();
        var timeStamps = new List<UtcTime>();
        var random = new Random();
        points.Add(new(30, 114));
        timeStamps.Add(UtcTime.Now);
        for (int i = 1; i < 1000; i++)
        {
            var lat = 0.0002 * random.NextDouble() + points[^1].Lat - 0.0001;
            var lng = 0.0002 * random.NextDouble() + points[^1].Lng - 0.0001;
            var point = new PointLatLng(lat, lng);
            var timeStamp = UtcTime.Now + i * TimeSpan.FromSeconds(3);
            points.Add(point);
            timeStamps.Add(timeStamp);
        }
        for (int i = 0; i < points.Count; i++)
        {
            _gMapRouteDisplayService.AddPoint(points[i], timeStamps[i]);
        }
        _gMapRouteDisplayService.StartAsync();
    }

    [RelayCommand]
    void Pause()
    {
        _gMapRouteDisplayService.Pause();
    }
}
