using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;
using GMap.NET;
using GMap.NET.MapProviders;
using Microsoft.Extensions.DependencyInjection;
using MiraiNavi.WpfApp.Services;
using MiraiNavi.WpfApp.Services.Contracts;
using MiraiNavi.WpfApp.ViewModels.Pages;
using MiraiNavi.WpfApp.ViewModels.Windows;
using MiraiNavi.WpfApp.Views.Pages;
using MiraiNavi.WpfApp.Views.Windows;
using Syncfusion.Licensing;
using Syncfusion.SfSkinManager;
using Syncfusion.Themes.FluentDark.WPF;
using Syncfusion.Themes.Windows11Dark.WPF;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

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

    public bool ApplyWindowBackdrop(BackgroundType type)
    {
        if (!Background.IsSupported(type)) return false;
        var preType = Windows.OfType<MainWindow>().First().WindowBackdropType;
        void SetWindowsBackdropType(BackgroundType setType)
        {
            foreach (var window in Windows)
                if (window is UiWindow uiWindow)
                    uiWindow.WindowBackdropType = setType;
        }

        try
        {
            SetWindowsBackdropType(type);
            return true;
        }
        catch (Exception)
        {
            SetWindowsBackdropType(preType);
            return false;
        }
    }

    public static void ApplyTheme()
    {
        Accent.Apply((Color)ColorConverter.ConvertFromString("#FF39c5bb"));
        Wpf.Ui.Appearance.Theme.Apply(ThemeType.Dark, BackgroundType.Auto, false, true);
        SfSkinManager.RegisterThemeSettings("FluentDark", new FluentDarkThemeSettings()
        {
            PrimaryBackground = Accent.PrimaryAccentBrush,
            FontFamily = new("Microsoft YaHei UI"),
            PrimaryForeground = Brushes.White,
        });
    }

    static void RegisterKeys()
    {
        var syncKey = "Mjk5OTQ4MEAzMjM0MmUzMDJlMzBoZlRQNFJpUC8xNXNBM09RUTUxa2tWYzdweUNtYUhJMDZiVXV0NlpSR1ZvPQ==";
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
        .AddTransient<ChartPageViewModel>()
        .AddTransient<ChartGroupPageViewModel>()
        .AddSingleton<SatelliteTrackingPageViewModel>()
        .AddSingleton<OutputPageViewModel>()
        .AddSingleton<MapPageViewModel>()
        .AddSingleton<SkyMapPageViewModel>()
        .AddSingleton<DashBoardPageViewModel>()
        .AddSingleton<PosePageViewModel>()
        .AddSingleton<MainWindowViewModel>()
        .AddSingleton<PropertyPageViewModel>()
        .AddTransient<ChartToolWindowViewModel>()
        .AddSingleton<SatelliteTrackingPage>()
        .AddSingleton<OutputPage>()
        .AddSingleton<DashBoardPage>()
        .AddSingleton<MapPage>()
        .AddSingleton<SkyMapPage>()
        .AddSingleton<PosePage>()
        .AddSingleton<MainWindow>()
        .AddSingleton<PropertyPage>()
        .AddTransient<ChartToolWindow>()
        .AddTransient<ChartPage>()
        .AddTransient<ChartGroupPage>()
        .BuildServiceProvider();

    protected override void OnStartup(StartupEventArgs e)
    {
        new SplashScreen("Assets/splash-screen.png").Show(true);
        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
        ApplyWindowBackdrop(BackgroundType.Acrylic);
        mainWindow.Show();
        mainWindow.WindowState = WindowState.Maximized;
        // TODO DEBUG
        mainWindow.ViewModel.Options = new("调试");
    }
}
