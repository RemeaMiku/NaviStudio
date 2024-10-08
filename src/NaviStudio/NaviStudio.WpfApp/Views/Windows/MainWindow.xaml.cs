﻿using System.Collections.Specialized;
using System.Diagnostics;
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
using NaviStudio.WpfApp.Services.Contracts;
using IDialogService = NaviStudio.WpfApp.Services.Contracts.IDialogService;
using Windows.UI.Popups;
using System.Linq;
using Wpf.Ui.Appearance;
using Syncfusion.SfSkinManager;
using Theme = Wpf.Ui.Appearance.Theme;
using NaviStudio.WpfApp.Common.Extensions;

namespace NaviStudio.WpfApp.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : UiWindow
{
    #region Public Constructors

    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        App.Current.ApplyTheme();
        App.Current.ApplyBackground();
        App.Current.Services.GetRequiredService<ISnackbarService>().SetSnackbarControl(Snackbar);
        App.Current.Services.GetRequiredKeyedService<IDialogService>(nameof(DynamicContentDialog)).RegisteDialog(DynamicContentDialog);
        App.Current.Services.GetRequiredKeyedService<IDialogService>(nameof(MessageDialog)).RegisteDialog(MessageDialog);
        SetPages();
        DockingManagerLayoutHelper.Register(DockingManagerControl);
        DockingManagerLayoutHelper.SaveDefault();
        DockingManagerControl.LoadDockState();
        DockingManagerLayoutHelper.Load();
        ViewModel = viewModel;
        ViewModel.LayoutNames = DockingManagerLayoutHelper.GetLayoutNames().ToList();
        ViewModel.IsActive = true;
        DataContext = this;
        OnThemeChanged(Theme.GetAppTheme(), default);
        Theme.Changed += OnThemeChanged;
    }

    private void OnThemeChanged(ThemeType currentTheme, System.Windows.Media.Color _)
    {
        var sfThemeName = currentTheme switch
        {
            ThemeType.Light => "FluentLight",
            ThemeType.Dark => "FluentDark",
            _ => throw new NotImplementedException(),
        };
        SfSkinManager.SetTheme(DockingManagerControl, new FluentTheme() { ThemeName = sfThemeName });
    }

    #endregion Public Constructors

    #region Public Properties

    public MainWindowViewModel ViewModel { get; }

    #endregion Public Properties

    #region Public Methods

    public void AddDocument(string header, object content, bool canSerialize = false)
    {
        var contentControl = new ContentControl()
        {
            Content = content,
            Style = (Style)Resources["DefaultContentControlStyle"],
        };
        DockingManagerControl.Children.Add(contentControl);
        DockingManager.SetState(contentControl, DockState.Document);
        DockingManager.SetHeader(contentControl, header);
        DockingManager.SetCanSerialize(contentControl, canSerialize);
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
        if(contentControl.Content is ChartGroupPage)
            DockingManagerControl.Children.Remove(contentControl);
    }

    void OnDockingManagerDocumentWindowClosed(object sender, CloseButtonEventArgs e)
    {
        var contentControl = (ContentControl)e.TargetItem;
        DockingWindowHandler.SaveDockState(contentControl);
        DockingWindowHandler.SetViewModelIsActive(contentControl, false);
        if(contentControl.Content is ChartGroupPage)
            DockingManagerControl.Children.Remove(contentControl);
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
            var paras = new ChartGroupParameters(window.ViewModel.ChartGroupName!, window.ViewModel.EpochCount, window.ViewModel.SelectedItems);
            var page = App.Current.Services.GetRequiredService<ChartGroupPage>();
            page.CreateCharts(paras);
            AddDocument($"图表组: {paras.Title}", page);
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

    private void OnOpenRealTimeOptionsFileItemClicked(object sender, RoutedEventArgs e)
    {
        OnRealTimeOptionsViewButtonClicked(sender, e);
        App.Current.Services.GetRequiredService<RealTimeOptionsPage>().ViewModel.ReadCommand.Execute(default);
    }

    private void OnOpenEpochDatasButtonClicked(object sender, RoutedEventArgs e)
    {
        DockingWindowHandler.RestoreDockState(MapView);
    }

    private void OnSaveLayoutMenuItemClick(object sender, RoutedEventArgs e)
    {
        DynamicContentDialog.Content = Resources["SaveLayoutDialogContent"];
        DynamicContentDialog.DialogHeight = 250;
    }

    private void OnManageLayoutsMenuItemClicked(object sender, RoutedEventArgs e)
    {
        DynamicContentDialog.Content = Resources["ManageLayoutsDialogContent"];
        DynamicContentDialog.DialogHeight = 400;
    }
}
