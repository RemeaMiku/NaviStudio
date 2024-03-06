using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using Microsoft.Extensions.DependencyInjection;
using NaviStudio.WpfApp.Services.Contracts;
using NaviStudio.WpfApp.ViewModels.Pages;

namespace NaviStudio.WpfApp.Views.Pages;

/// <summary>
/// MapPage.xaml 的交互逻辑
/// </summary>
public partial class MapPage : UserControl
{
    #region Public Constructors

    public MapPage(MapPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        DataContext = this;
        InitializeGMap();
        var shape = (FrameworkElement)Resources["SelectedPointMarkerShape"];
        _selectedPointMarker = new GMapMarker(new())
        {
            Shape = shape,
            Offset = new(0, -20)
        };
        // 比例尺
        //GMap.OnMapZoomChanged += () => Test.Text = GMap.MapProvider.Projection.GetGroundResolution((int)GMap.Zoom, GMap.Position.Lat).ToString();
    }

    #endregion Public Constructors

    #region Public Properties

    public MapPageViewModel ViewModel { get; }

    #endregion Public Properties

    #region Private Fields

    readonly GMapMarker _selectedPointMarker;

    #endregion Private Fields

    #region Private Methods

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if(e.PropertyName == nameof(ViewModel.MapCenter))
            App.Current.Dispatcher.Invoke(() => GMap.CenterPosition = ViewModel.MapCenter);
        else if(e.PropertyName == nameof(ViewModel.KeepCenter))
        {
            GMap.MouseWheelZoomType = ViewModel.KeepCenter ? MouseWheelZoomType.ViewCenter : MouseWheelZoomType.MousePositionWithoutCenter;
        }
    }
    private void InitializeGMap()
    {
        App.Current.Services.GetRequiredService<IGMapRouteDisplayService>().RegisterGMapControl(GMap);
        GMap.ShowCenter = false;
        GMap.DragButton = MouseButton.Right;
    }
    private void OnGMapMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        GMap.Markers.Remove(_selectedPointMarker);
        ViewModel.SelectedPoint = default;
        var mousePosition = e.GetPosition(GMap);
        var location = GMap.FromLocalToLatLng((int)mousePosition.X, (int)mousePosition.Y);
        VisualTreeHelper.HitTest(GMap, null, (r) =>
        {
            if(r.VisualHit is Ellipse e && e.Tag is not null)
            {
                ViewModel.SelectedPoint = (TimePointLatLng)e.Tag;
                _selectedPointMarker.Position = ViewModel.SelectedPoint.Value.Item2;
                GMap.Markers.Add(_selectedPointMarker);
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

    private void OnGMapZoomChanged()
    {
        if(ViewModel.KeepCenter)
            ViewModel.ReturnToPositionCommand.Execute(default);
    }
    #endregion Private Methods


}
