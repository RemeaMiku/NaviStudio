using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using NaviStudio.Shared.Models.Chart;
using NaviStudio.WpfApp.Common.Helpers;
using NaviStudio.WpfApp.ViewModels.Windows;
using NaviStudio.WpfApp.Views.Pages;
using Syncfusion.Windows.Tools.Controls;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace NaviStudio.WpfApp.Views.Windows;

//TODO 关闭时保存子窗口布局

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : UiWindow
{
    #region Public Constructors

    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        App.Current.SettingsManager.TryApplyAcrylicIfIsEnabled(this);
        App.Current.Services.GetRequiredService<ISnackbarService>().SetSnackbarControl(Snackbar);
        ViewModel = viewModel;
        ViewModel.IsActive = true;
        DataContext = this;
        SetPages();
    }

    #endregion Public Constructors

    #region Public Properties

    public MainWindowViewModel ViewModel { get; }

    #endregion Public Properties

    #region Public Methods

    public void AddDocument(string header, object content)
    {
        var contentControl = new ContentControl()
        {
            Content = content,
            Style = (Style)Resources["DefaultContentControlStyle"],
        };
        DockingManagerControl.Children.Add(contentControl);
        DockingManager.SetState(contentControl, DockState.Document);
        DockingManager.SetHeader(contentControl, header);
    }

    #endregion Public Methods

    #region Private Methods

    void SetPages()
    {
        RealTimeOptionsView.Content = App.Current.Services.GetRequiredService<RealTimeOptionsPage>();
        MapView.Content = App.Current.Services.GetRequiredService<MapPage>();
        SkyMapView.Content = App.Current.Services.GetRequiredService<SkyMapPage>();
        PoseView.Content = App.Current.Services.GetRequiredService<PosePage>();
        DashBoardView.Content = App.Current.Services.GetRequiredService<DashBoardPage>();
        OutputView.Content = App.Current.Services.GetRequiredService<OutputPage>();
        SatelliteTrackingView.Content = App.Current.Services.GetRequiredService<SatelliteTrackingPage>();
        PropertyView.Content = App.Current.Services.GetRequiredService<PropertyPage>();
    }
    void RestoreAndActiveWindow(ContentControl contentControl)
    {
        if(DockingManager.GetState(contentControl) == DockState.Hidden)
            DockingWindowHandler.RestoreDockState(contentControl);
        else
            DockingManagerControl.ActiveWindow = contentControl;
    }

    void OnViewMenuItemClicked(object sender, RoutedEventArgs e)
    {
        var contentControl = (ContentControl)((Wpf.Ui.Controls.MenuItem)sender).Tag;
        RestoreAndActiveWindow(contentControl);
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
        if(e.Action == NotifyCollectionChangedAction.Add)
        {
            if(e.NewItems is null)
                return;
            foreach(ContentControl item in e.NewItems)
                DockingWindowHandler.SetViewModelIsActive(item, true);
        }
    }

    private void DockingManagerControl_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void OnChartToolItemClicked(object sender, RoutedEventArgs e)
    {
        var window = App.Current.Services.GetRequiredService<ChartToolWindow>();
        if(window.ShowDialog() == true)
        {
            var paras = new ChartGroupParameters(window.ViewModel.ChartGroupName!, window.ViewModel.MaxEpochCount, window.ViewModel.SelectedItems);
            var page = App.Current.Services.GetRequiredService<ChartGroupPage>();
            AddDocument($"图表组: {paras.Title}", page);
            page.CreateCharts(paras);
        }
    }

    private void OnRealTimeOptionsViewButtonClicked(object sender, RoutedEventArgs e)
    {
        RestoreAndActiveWindow(RealTimeOptionsView);
    }

    private void OnAppSettingsItemClicked(object sender, RoutedEventArgs e)
    {
        App.Current.Services.GetRequiredService<AppSettingsWindow>().ShowDialog();
    }

    #endregion Private Methods
}
