using System.Windows.Controls;
using MiraiNavi.WpfApp.ViewModels.Pages;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// StartPage.xaml 的交互逻辑
/// </summary>
public partial class RealTimeOptionsPage : UserControl
{
    #region Public Constructors

    public RealTimeOptionsPage(RealTimeOptionsPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = this;
        ViewModel = viewModel;
    }

    #endregion Public Constructors

    #region Public Properties

    public RealTimeOptionsPageViewModel ViewModel { get; }

    #endregion Public Properties
}
