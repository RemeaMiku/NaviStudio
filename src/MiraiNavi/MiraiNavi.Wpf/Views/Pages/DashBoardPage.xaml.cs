using System.Windows.Controls;
using MiraiNavi.WpfApp.ViewModels.Pages;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// DashBoardPage.xaml 的交互逻辑
/// </summary>
public partial class DashBoardPage : UserControl
{
    public DashBoardPage(DashBoardPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = this;
        ViewModel = viewModel;
    }

    public DashBoardPageViewModel ViewModel { get; }

    private void OnCompassLabelCreated(object sender, Syncfusion.UI.Xaml.Gauges.LabelCreatedEventArgs e)
    {
        e.LabelText = e.LabelText switch
        {
            "0" => "N",
            "90" => "E",
            "180" => "S",
            "270" => "W",
            _ => e.LabelText
        };
    }
}
