using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GMap.NET;
using GMap.NET.MapProviders;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// MapPage.xaml 的交互逻辑
/// </summary>
public partial class MapPage : UserControl
{
    public MapPage()
    {
        InitializeComponent();
    }

    private void OnGMapPositionChanged(PointLatLng point)
    {
        // MessageBox.Show(GMap.ViewArea.ToString());
    }

    private void OnGMapLoaded(object sender, RoutedEventArgs e)
    {
        GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
        GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
        GMaps.Instance.Mode = AccessMode.ServerAndCache;
        // choose your provider here        
        GMapProvider.Language = LanguageType.ChineseSimplified;
        GMap.MapProvider = BingHybridMapProvider.Instance;
        GMap.ShowCenter = false;
        // lets the user drag the map with the left mouse button
        GMap.DragButton = MouseButton.Right;
        GMap.CenterPosition = new(30, 114);
    }

    private void OnGMap2Loaded(object sender, RoutedEventArgs e)
    {
        // GMap2.MapProvider = BingSatelliteMapProvider.Instance;
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        GMap.MapProvider = ((ComboBoxItem)MapTypeBox.SelectedItem).Content.ToString() switch
        {
            "混合" => BingHybridMapProvider.Instance,
            "卫星" => BingSatelliteMapProvider.Instance,
            "街道" => BingMapProvider.Instance,
            _ => throw new NotImplementedException(),
        };
    }
}
