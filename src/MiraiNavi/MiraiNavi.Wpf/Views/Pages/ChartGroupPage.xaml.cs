using System;
using System.Collections.Frozen;
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
using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.Models;
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
        var count = 0;
        foreach (var item in groupParas.Items)
        {
            var page = App.Current.ServiceProvider.GetRequiredService<ChartPage>();
            var paras = ChartParameters.FromChartItem(item);
            page.CreateSeries(paras);
            ViewModel.ChartParas.Add(page.ViewModel, paras);
            DocumentContainer.SetHeader(page, item);
            DocumentContainer.SetCanClose(page, false);
            DocumentContainer.SetMDIWindowState(page, MDIWindowState.Normal);
            DocumentContainer.SetMDIBounds(page, new(count * 100, count * 50, 400, 200));
            DocumentContainerControl.Items.Add(page);
            count++;
        }
    }

    public ChartGroupPageViewModel ViewModel { get; }
}
