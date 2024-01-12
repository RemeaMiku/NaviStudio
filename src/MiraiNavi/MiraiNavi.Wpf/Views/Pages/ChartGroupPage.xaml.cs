using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Models.Chart;
using MiraiNavi.WpfApp.ViewModels.Pages;
using Syncfusion.Windows.Tools.Controls;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// ChartGroupPage.xaml 的交互逻辑
/// </summary>
public partial class ChartGroupPage : UserControl
{
    public ChartGroupPage(ChartGroupPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = this;
    }

    public void CreateChartPages(ChartGroupParameters groupParas)
    {
        ViewModel.Title = groupParas.Title;
        ViewModel.MaxEpochCount = groupParas.MaxEpochCount;
        var index = 0;
        foreach (var item in groupParas.Items)
        {
            var page = App.Current.Services.GetRequiredService<ChartPage>();
            var paras = ChartParameters.FromChartItem(item);
            if (paras is null)
                continue;
            page.CreateSeries(paras);
            ViewModel.ChartParas.Add(page.ViewModel, paras);
            DocumentContainer.SetHeader(page, item);
            DocumentContainer.SetCanClose(page, false);
            DocumentContainer.SetMDIWindowState(page, MDIWindowState.Normal);
            DocumentContainer.SetMDIBounds(page, new(index * 100, index * 50, 400, 200));
            DocumentContainerControl.Items.Add(page);
            index++;
        }
    }

    public ChartGroupPageViewModel ViewModel { get; }

    private void OnLayoutButtonClicked(object sender, RoutedEventArgs e)
    {
        DocumentContainerControl.SetLayout((MDILayout)((Button)sender).Tag);
    }
}
