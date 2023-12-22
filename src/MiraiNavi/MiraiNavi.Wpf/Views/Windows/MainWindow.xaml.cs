using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.Common.Helpers;
using MiraiNavi.WpfApp.ViewModels.Windows;
using MiraiNavi.WpfApp.Views.Pages;
using Syncfusion.Windows.Tools.Controls;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Views.Windows;

//TODO 关闭子窗口时，不激活子窗口ViewModel；关闭时保存子窗口布局

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : UiWindow
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = this;
        Loaded += OnMainWindowLoaded;
    }

    private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
    {
        App.ApplyTheme();
        SetPages();
        ViewModel.IsActive = true;
    }

    void SetPages()
    {
        MapView.Content = App.Current.ServiceProvider.GetRequiredService<MapPage>();
        SkyMapView.Content = App.Current.ServiceProvider.GetRequiredService<SkyMapPage>();
        PoseView.Content = App.Current.ServiceProvider.GetRequiredService<PosePage>();
        DashBoardView.Content = App.Current.ServiceProvider.GetRequiredService<DashBoardPage>();
        OutputView.Content = App.Current.ServiceProvider.GetRequiredService<OutputPage>();
    }

    public MainWindowViewModel ViewModel { get; }

    void OnViewMenuItemClicked(object sender, RoutedEventArgs e)
    {
        var contentControl = (ContentControl)((Wpf.Ui.Controls.MenuItem)sender).Tag;
        if (DockingManager.GetState(contentControl) == DockState.Hidden)
            DockingWindowHandler.RestoreDockState(contentControl);
        else
            DockingManagerControl.ActiveWindow = contentControl;
    }

    void OnDockingManagerNotDocumentWindowClosing(object sender, WindowClosingEventArgs e)
    {
        var contentControl = (ContentControl)e.TargetItem;
        DockingWindowHandler.SaveDockState(contentControl);
        DockingWindowHandler.SetViewModelIsActive(contentControl, false);
    }

    void OnDockingManagerDocumentWindowClosed(object sender, CloseButtonEventArgs e)
    {
        var contentControl = (ContentControl)e.TargetItem;
        DockingWindowHandler.SaveDockState(contentControl);
        DockingWindowHandler.SetViewModelIsActive(contentControl, false);
    }

    void OnDockingManagerChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            if (e.NewItems is null)
                return;
            foreach (ContentControl item in e.NewItems)
                DockingWindowHandler.SetViewModelIsActive(item, true);
        }
    }

    private void DockingManagerControl_Loaded(object sender, RoutedEventArgs e)
    {

    }
}
