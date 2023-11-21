﻿using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.ViewModels.Windows;
using MiraiNavi.WpfApp.Views.Pages;
using Syncfusion.Windows.Tools.Controls;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : UiWindow
{
    readonly Dictionary<ContentControl, DockState> _dockStatesOnClosed = [];

    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        Wpf.Ui.Appearance.Theme.Apply(Wpf.Ui.Appearance.ThemeType.Dark, Wpf.Ui.Appearance.BackgroundType.Acrylic, true, true);
        SetPages();
    }

    void SetPages()
    {
        SkyMapView.Content = App.Current.ServiceProvider.GetRequiredService<SkyMapPage>();
        NavigationParameterView.Content = App.Current.ServiceProvider.GetRequiredService<NavigationParameterPage>();
    }

    public MainWindowViewModel ViewModel { get; }

    void OnViewMenuItemClicked(object sender, System.Windows.RoutedEventArgs e)
    {
        var contentControl = (ContentControl)((Wpf.Ui.Controls.MenuItem)sender).Tag;
        if (DockingManager.GetState(contentControl) == DockState.Hidden)
            DockingManager.SetState(contentControl, _dockStatesOnClosed[contentControl!]);
        else
            DockingManagerControl.ActiveWindow = contentControl;
    }

    void SaveDockState(ContentControl contentControl) => _dockStatesOnClosed[contentControl] = DockingManager.GetState(contentControl);

    void OnDockingManagerControlWindowClosing(object sender, WindowClosingEventArgs e) => SaveDockState((ContentControl)e.TargetItem);

    void OnDockingManagerControlCloseButtonClicked(object sender, CloseButtonEventArgs e) => SaveDockState((ContentControl)e.TargetItem);
}
