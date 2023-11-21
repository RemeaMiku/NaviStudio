using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.Services.Contracts;
using MiraiNavi.WpfApp.Services.DesignTime;
using MiraiNavi.WpfApp.ViewModels.Pages;
using MiraiNavi.WpfApp.ViewModels.Windows;
using MiraiNavi.WpfApp.Views.Pages;
using MiraiNavi.WpfApp.Views.Windows;
using Syncfusion.SfSkinManager;
using Syncfusion.Themes.FluentDark.WPF;

namespace MiraiNavi.WpfApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        //Register Syncfusion license
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cXmVCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH5ecXZWQmhfWUZzV0A=");
        SfSkinManager.RegisterThemeSettings("FluentDark", new FluentDarkThemeSettings()
        {
            Palette = FluentPalette.Cyan,
            FontFamily = new("Microsoft Yahei UI")
        });

    }

    public static new App Current => (App)Application.Current;

    public IServiceProvider ServiceProvider { get; } = new ServiceCollection()
        .AddSingleton<ISatelliteServcie, DesignTimeSatelliteService>()
        .AddSingleton<SkyMapPageViewModel>()
        .AddSingleton<NavigationParameterPageViewModel>()
        .AddSingleton<MainWindowViewModel>()
        .AddSingleton<SkyMapPage>()
        .AddSingleton<NavigationParameterPage>()
        .AddSingleton<MainWindow>()
        .BuildServiceProvider();

    protected override void OnStartup(StartupEventArgs e) => ServiceProvider.GetRequiredService<MainWindow>().Show();
}
