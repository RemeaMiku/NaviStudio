using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.Services.Contracts;
using MiraiNavi.WpfApp.ViewModels.Pages;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// MapPage.xaml 的交互逻辑
/// </summary>
public partial class MapPage : UserControl
{
    public MapPage(MapPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        DataContext = this;
        InitializeGMap();
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ViewModel.MapCenter))
            App.Current.Dispatcher.Invoke(() => GMap.CenterPosition = ViewModel.MapCenter);
    }

    public MapPageViewModel ViewModel { get; }

    private void InitializeGMap()
    {
        var shape = new Ellipse
        {
            Stroke = Brushes.White,
            StrokeThickness = 3,
            Width = 15,
            Height = 15,
            Fill = (Brush)App.Current.Resources["MikuMeaRedBlueGreenHorizontalBrush"],
        };
        var marker = new GMapMarker(new PointLatLng(0, 0))
        {
            Shape = shape,
            Offset = new(-shape.Width / 2, -shape.Height / 2)
        };
        App.Current.ServiceProvider.GetRequiredService<IGMapRouteDisplayService>()
            .RegisterGMapControl(GMap)
            .RegisterPositionMarker(marker);
        GMap.ShowCenter = false;
        GMap.DragButton = MouseButton.Right;
    }

    private void OnGMapMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        //TODO:测试点击Marker：VisualTreeHelper.HitTest
        var mousePosition = e.GetPosition(GMap);
        //var location = GMap.FromLocalToLatLng((int)mousePosition.X, (int)mousePosition.Y);
        VisualTreeHelper.HitTest(GMap, null, (r) =>
        {
            if (r.VisualHit is Ellipse e && e.Tag is not null)
            {
                (var location, var timeStamp) = ((PointLatLng, UtcTime))e.Tag;
                MessageBox.Show($"{timeStamp:yyyy/MM/dd HH:mm:ss.fff}{Environment.NewLine}纬度：{location.Lat}, 经度：{location.Lng}");
                return HitTestResultBehavior.Stop;
            }
            return HitTestResultBehavior.Continue;
        }, new PointHitTestParameters(mousePosition));

    }

    private void OnGMapOnPositionChanged(PointLatLng point)
    {
        ViewModel.MapCenter = point;
    }

    private void OnGMapDrag()
    {
        ViewModel.KeepCenter = false;
    }
}
