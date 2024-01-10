using System.Windows.Controls;
using MiraiNavi.WpfApp.Common.Helpers;
using MiraiNavi.WpfApp.ViewModels.Pages;
using Syncfusion.Windows.PropertyGrid;

namespace MiraiNavi.WpfApp.Views.Pages;

/// <summary>
/// PropertyPage.xaml 的交互逻辑
/// </summary>
public partial class PropertyPage : UserControl
{
    public PropertyPage(PropertyPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = this;
        ViewModel = viewModel;
    }

    public PropertyPageViewModel ViewModel { get; }

    private void OnPropertyGridAutoGeneratingPropertyGridItem(object sender, AutoGeneratingPropertyGridItemEventArgs e)
    {
        if (!PropertyDescriptionManager.Descriptions.TryGetValue(e.DisplayName, out var description))
        {
            e.Cancel = true;
            return;
        }
        e.DisplayName = description.DisplayName;
        e.Description = description.Description;
        e.Category = description.Category;
        e.ReadOnly = true;
    }
}