using System.Windows.Controls;
using MiraiNavi.WpfApp.ViewModels.Pages;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// DashBoardPage.xaml 的交互逻辑
/// </summary>
public partial class DashBoardPage : UserControl
{
    #region Public Constructors

    public DashBoardPage(DashBoardPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = this;
        ViewModel = viewModel;
    }

    #endregion Public Constructors

    #region Public Properties

    public DashBoardPageViewModel ViewModel { get; }

    #endregion Public Properties

    #region Private Methods

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

    #endregion Private Methods
}
