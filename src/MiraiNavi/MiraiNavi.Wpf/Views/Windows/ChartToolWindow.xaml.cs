using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.Common.Helpers;
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
        if (AppSettingsManager.Settings.AppearanceSettings.EnableAcrylic)
            AppSettingsManager.TryApplyAcrylicIfIsEnabled(this);
        ViewModel = viewModel;
        DataContext = this;
        ViewModel.CreateRequested += (sender, paras) =>
        {
            var page = App.Current.Services.GetRequiredService<ChartGroupPage>();
            App.Current.Services.GetRequiredService<MainWindow>().AddDocument($"图表组: {paras.Title}", page);
            page.CreateCharts(paras);
            Close();
        };
    }

    public ChartToolWindowViewModel ViewModel { get; }

}
