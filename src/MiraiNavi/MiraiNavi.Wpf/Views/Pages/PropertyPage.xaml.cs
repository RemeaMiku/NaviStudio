using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Controls;
using MiraiNavi.Shared.Models.Navi;
using MiraiNavi.Shared.Models.Satellites;
using MiraiNavi.Shared.Models.Solution;
using MiraiNavi.WpfApp.Common.Extensions;
using MiraiNavi.WpfApp.Common.Helpers;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.ViewModels.Pages;
using NaviSharp;
using NaviSharp.SpatialReference;
using Syncfusion.Windows.Controls.Input;
using Syncfusion.Windows.PropertyGrid;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// PropertyPage.xaml 的交互逻辑
/// </summary>
public partial class PropertyPage : UserControl
{
    public PropertyPage(PropertyPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = this;
        ViewModel = viewModel;
        PropertyGrid.AutoGenerateItems = false;
        //var options = new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        //var json = JsonSerializer.Serialize(new EpochData()
        //{
        //    Result = new(),
        //    Precision = new(),
        //    QualityFactors = new(),
        //    TimeStamp = UtcTime.Now,
        //    SatelliteSkyPositions = new List<SatelliteSkyPosition>() { new() { Satellite = "C01", Azimuth = Angle.FromDegrees(27), Elevation = Angle.FromDegrees(27) } },
        //    SatelliteTrackings = new List<SatelliteTracking>() { new() { Satellite = "C01", Frequency = 1111, IsUsed = true, SignalNoiseRatio = 111 } },
        //}, options);
        //var epochData = JsonSerializer.Deserialize<EpochData>(json, options);
        //PropertyGrid.SelectedObject = epochData;
    }

    public PropertyPageViewModel ViewModel { get; }

    private void OnPropertyGridAutoGeneratingPropertyGridItem(object sender, AutoGeneratingPropertyGridItemEventArgs e)
    {
        if (!PropertyDescriptionManager.Descriptions.TryGetValue(e.DisplayName, out var description))
        {
            e.Cancel = true;
            return;
        }
        e.DisplayName = description.DisplayName;
        e.Description = description.Description;
        e.Category = description.Category;
        e.ReadOnly = true;
    }
}