using System.Windows.Controls;
using System.Windows.Data;
using NaviStudio.WpfApp.ViewModels.Pages;

namespace NaviStudio.WpfApp.Views.Pages;

/// <summary>
/// OuputPage.xaml 的交互逻辑
/// </summary>
public partial class OutputPage : UserControl
{
    #region Public Constructors

    public OutputPage(OutputPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = this;
        ViewModel.OutputsView = CollectionViewSource.GetDefaultView(ViewModel.Outputs);
        ViewModel.OutputsView.Filter = ViewModel.OutputsViewFilter;
        ViewModel.ScrollToBottomRequested += OnViewModelScrollToBottomRequested;
    }

    #endregion Public Constructors

    #region Public Properties

    public OutputPageViewModel ViewModel { get; }

    #endregion Public Properties

    #region Private Methods

    private void OnViewModelScrollToBottomRequested(object? sender, EventArgs e)
    {
        App.Current.Dispatcher.Invoke(() =>
        {
            if (OutputList.Items.Count > 0)
                OutputList.ScrollIntoView(OutputList.Items[^1]);
        });
    }

    #endregion Private Methods
}
