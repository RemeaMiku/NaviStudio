using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;
using GMap.NET;
using GMap.NET.MapProviders;
using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.Services;
using MiraiNavi.WpfApp.Services.Contracts;
using MiraiNavi.WpfApp.Services.Test;
using MiraiNavi.WpfApp.ViewModels.Pages;
using MiraiNavi.WpfApp.ViewModels.Windows;
using MiraiNavi.WpfApp.Views.Pages;
using MiraiNavi.WpfApp.Views.Windows;
using Syncfusion.Licensing;
using Syncfusion.SfSkinManager;
using Syncfusion.Themes.FluentDark.WPF;
using Wpf.Ui.Appearance;

namespace MiraiNavi.WpfApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
        GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
        GMaps.Instance.Mode = AccessMode.ServerAndCache;
        GMapProvider.Language = LanguageType.ChineseSimplified;
        RegisterKeys();
        ApplyTheme();
    }

    public static void ApplyTheme()
    {
        Accent.Apply((Color)ColorConverter.ConvertFromString("#FF39c5bb"));
        Wpf.Ui.Appearance.Theme.Apply(ThemeType.Dark, BackgroundType.Acrylic, false, true);
        SfSkinManager.RegisterThemeSettings("FluentDark", new FluentDarkThemeSettings()
        {
            PrimaryBackground = Accent.PrimaryAccentBrush,
            FontFamily = new("Microsoft YaHei UI")
        });
    }

    static void RegisterKeys()
    {
        //注册 Syncfusion 控件
        var syncKey = "Ngo9BigBOggjHTQxAR8/V1NAaF5cWWJCflFrRWJEfV5ycEVHaFZTQ3xbR1ZhSXxQd0dhX35WcnNRQ2ZbV0I=";
        //注册 GMap.NET Bing Map Key
        var bingKey = "AlIHhkb_-Q9xEyaWGoVmIhsVQPM1W7KCY0jGPLrio-gBFxny155gdrjwXllhuRYN";
        SyncfusionLicenseProvider.RegisterLicense(syncKey);
        BingSatelliteMapProvider.Instance.ClientKey = bingKey;
    }

    public static new App Current => (App)Application.Current;


    public IServiceProvider ServiceProvider { get; } = new ServiceCollection()
        .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)
        .AddSingleton<IRealTimeControlService, TcpJsonRealTimeControlService>()
        .AddSingleton<IEpochDatasService, EpochDatasService>()
        .AddSingleton<IGMapRouteDisplayService, GMapRouteDisplayService>()
        .AddSingleton<SatelliteTrackingPageViewModel>()
        .AddSingleton<OutputPageViewModel>()
        .AddSingleton<MapPageViewModel>()
        .AddSingleton<SkyMapPageViewModel>()
        .AddSingleton<DashBoardPageViewModel>()
        .AddSingleton<PosePageViewModel>()
        .AddSingleton<MainWindowViewModel>()
        .AddSingleton<SatelliteTrackingPage>()
        .AddSingleton<OutputPage>()
        .AddSingleton<DashBoardPage>()
        .AddSingleton<MapPage>()
        .AddSingleton<SkyMapPage>()
        .AddSingleton<PosePage>()
        .AddSingleton<MainWindow>()
        .BuildServiceProvider();

    protected override void OnStartup(StartupEventArgs e)
    {
#if DEBUG
        ServiceProvider.GetRequiredService<MainWindowViewModel>().RealTimeControlOptions = new("文件模拟");
#endif
        new SplashScreen("Assets/splash-screen.png").Show(true);
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
