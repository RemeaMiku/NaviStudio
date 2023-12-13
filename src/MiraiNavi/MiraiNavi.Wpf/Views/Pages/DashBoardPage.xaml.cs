using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
