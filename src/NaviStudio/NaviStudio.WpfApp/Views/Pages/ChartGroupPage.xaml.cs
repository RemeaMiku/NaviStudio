using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using NaviStudio.Shared.Models.Chart;
using NaviStudio.WpfApp.Common.Helpers;
using NaviStudio.WpfApp.ViewModels.Pages;
using Syncfusion.Windows.Tools.Controls;

namespace NaviStudio.WpfApp.Views.Pages;

/// <summary>
/// ChartGroupPage.xaml 的交互逻辑
/// </summary>
public partial class ChartGroupPage : UserControl
{
    #region Public Constructors

    public ChartGroupPage(ChartGroupPageViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = this;
    }

    #endregion Public Constructors

    #region Public Properties

    public ChartGroupPageViewModel ViewModel { get; }

    #endregion Public Properties

    #region Public Methods

    public void CreateCharts(ChartGroupParameters groupParas)
    {
        ViewModel.Title = groupParas.Title;
        ViewModel.MaxEpochCount = groupParas.MaxEpochCount;
        var index = 0;
        foreach(var item in groupParas.Items)
        {
            var page = App.Current.Services.GetRequiredService<ChartPage>();
            page.ViewModel.Title = item;
            ViewModel.ItemViewModels.Add(page.ViewModel);
            if(ChartItemManager.ChartItemFuncs.TryGetValue(item, out var funcs))
            {
                foreach((var label, _) in funcs)
                    page.AddSeries(label);
            }
            DocumentContainer.SetHeader(page, item);
            DocumentContainer.SetMDIWindowState(page, MDIWindowState.Normal);
            DocumentContainer.SetMDIBounds(page, new(index * 100, index * 50, 400, 200));
            DocumentContainerControl.Items.Add(page);
            index++;
        }
    }

    #endregion Public Methods

    #region Private Methods

    private void OnLayoutButtonClicked(object sender, RoutedEventArgs e)
    {
        DocumentContainerControl.SetLayout((MDILayout)((Button)sender).Tag);
    }

    #endregion Private Methods
}
