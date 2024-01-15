using System.Windows.Controls;
using System.Windows.Data;
using MiraiNavi.WpfApp.ViewModels.Pages;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// OuputPage.xaml 的交互逻辑
/// </summary>
public partial class OutputPage : UserControl
{
    public OutputPage(OutputPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = this;
        ViewModel.OutputsView = CollectionViewSource.GetDefaultView(ViewModel.Outputs);
        ViewModel.OutputsView.Filter = ViewModel.OutputsViewFilter;
        ViewModel.ScrollToBottomRequested += OnViewModelScrollToBottomRequested;
    }

    private void OnViewModelScrollToBottomRequested(object? sender, EventArgs e)
    {
        App.Current.Dispatcher.Invoke(() =>
        {
            if (OutputList.Items.Count > 0)
                OutputList.ScrollIntoView(OutputList.Items[^1]);
        });
    }

    public OutputPageViewModel ViewModel { get; }
}
