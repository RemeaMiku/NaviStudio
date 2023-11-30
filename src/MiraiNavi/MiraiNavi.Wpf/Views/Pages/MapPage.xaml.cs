using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
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
        DataContext = this;
        var shape = new Ellipse
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Stroke = Brushes.White,
            StrokeThickness = 3,
            Width = 15,
            Height = 15,
            Fill = (Brush)App.Current.Resources["MikuGreenBrush"],
        };
        var marker = new GMapMarker(new PointLatLng(0, 0))
        {
            Shape = shape,
            Offset = new(-shape.Width / 2, -shape.Height / 2)
        };
        App.Current.ServiceProvider.GetRequiredService<IGMapRouteReplayService>()
            .Initiliaze(GMap, marker, Brushes.Gray, (Brush)App.Current.Resources["MikuRedBrush"]);
    }

    public MapPageViewModel ViewModel { get; }

    private void OnGMapLoaded(object sender, RoutedEventArgs e)
    {
        GMap.ShowCenter = false;
        GMap.DragButton = MouseButton.Right;
    }

}
