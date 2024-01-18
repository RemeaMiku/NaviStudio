using System.Windows.Controls;
using MiraiNavi.Shared.Models.Options;
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
        if (!DisplayDescriptionManager.Descriptions.TryGetValue(e.DisplayName, out var description))
        {
            e.Cancel = true;
            return;
        }
        e.DisplayName = description.DisplayName;
        e.Description = description.Description;
        e.ReadOnly = true;
    }

    private void PropertyGrid_CollectionEditorOpening(object sender, CollectionEditorOpeningEventArgs e)
    {
        e.IsReadonly = true;
    }
}