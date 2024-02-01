using System.Windows.Controls;
using NaviStudio.WpfApp.ViewModels.Pages;

namespace NaviStudio.WpfApp.Views.Pages;

/// <summary>
/// NavigationParameterPage.xaml 的交互逻辑
/// </summary>
public partial class PosePage : UserControl
{
    #region Public Constructors

    public PosePage(PosePageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = this;
    }

    #endregion Public Constructors

    #region Public Properties

    public PosePageViewModel ViewModel { get; }

    #endregion Public Properties
}
