using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.ViewModels.Windows;
using MiraiNavi.WpfApp.Views.Pages;
using Syncfusion.SfSkinManager;
using Syncfusion.Themes.FluentDark.WPF;
using Syncfusion.Windows.Shared;
using Syncfusion.Windows.Tools.Controls;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : UiWindow, IHasViewModel<MainWindowViewModel>
{
    readonly Dictionary<ContentControl, DockState> _dockStatesOnClosed = new();

    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
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
