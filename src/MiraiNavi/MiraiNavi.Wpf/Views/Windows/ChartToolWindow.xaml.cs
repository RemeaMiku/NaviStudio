using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.ViewModels.Windows;
using MiraiNavi.WpfApp.Views.Pages;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Views.Windows;

/// <summary>
/// ChartToolWindowWindow.xaml 的交互逻辑
/// </summary>
public partial class ChartToolWindow : UiWindow
{
    public ChartToolWindow(ChartToolWindowViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = this;
        ViewModel.CreateRequested += (sender, paras) =>
        {
            var page = App.Current.ServiceProvider.GetRequiredService<ChartGroupPage>();
            App.Current.ServiceProvider.GetRequiredService<MainWindow>().AddDocument(paras.Title, page);
            page.CreateChartPages(paras);
            Close();
        };
    }

    public ChartToolWindowViewModel ViewModel { get; }

}
