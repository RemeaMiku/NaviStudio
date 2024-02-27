using System.Net;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;
using GMap.NET;
using GMap.NET.MapProviders;
using Microsoft.Extensions.DependencyInjection;
using NaviStudio.WpfApp.Common.Extensions;
using NaviStudio.WpfApp.Common.Settings;
using NaviStudio.WpfApp.Services;
using NaviStudio.WpfApp.Services.Contracts;
using NaviStudio.WpfApp.ViewModels.Pages;
using NaviStudio.WpfApp.ViewModels.Windows;
using NaviStudio.WpfApp.Views.Pages;
using NaviStudio.WpfApp.Views.Windows;
using Syncfusion.Licensing;
using Syncfusion.SfSkinManager;
using Syncfusion.Themes.FluentDark.WPF;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace NaviStudio.WpfApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{

    public static void SetGMap()
    {
        GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
        GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
        GMaps.Instance.Mode = AccessMode.ServerAndCache;
        GMapProvider.Language = LanguageType.ChineseSimplified;
    }

    public static void ApplyTheme()
    {
        //TODO 主题设置
        Accent.Apply((Color)ColorConverter.ConvertFromString("#FF39c5bb"));
        SfSkinManager.RegisterThemeSettings("FluentDark", new FluentDarkThemeSettings()
        {
            PrimaryBackground = Accent.PrimaryAccentBrush,
            FontFamily = new("Microsoft YaHei UI"),
            PrimaryForeground = Brushes.White,
        });
    }

    public bool TryApplyAcrylicToAllWindowsIfIsEnabled()
    {
        var flag = true;
        foreach(var window in Windows)
            if(window is UiWindow uiWindow)
                flag = flag && SettingsManager.TryApplyAcrylicIfIsEnabled(uiWindow);
        return flag;
    }

    static void RegisterKeys()
    {
        var syncKey = "Mjk5OTQ4MEAzMjM0MmUzMDJlMzBoZlRQNFJpUC8xNXNBM09RUTUxa2tWYzdweUNtYUhJMDZiVXV0NlpSR1ZvPQ==";
        var bingKey = "AlIHhkb_-Q9xEyaWGoVmIhsVQPM1W7KCY0jGPLrio-gBFxny155gdrjwXllhuRYN";
        SyncfusionLicenseProvider.RegisterLicense(syncKey);
        BingSatelliteMapProvider.Instance.ClientKey = bingKey;
    }

    public static new App Current => (App)Application.Current;

    public IServiceProvider Services { get; } = new ServiceCollection()
            .AddViews()
            .AddViewModels()
            .AddServices()
            .BuildServiceProvider();

    public AppSettingsManager SettingsManager { get; } = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        SetGMap();
        RegisterKeys();
        ApplyTheme();
        new SplashScreen("Assets/splash-screen.png").Show(true);
        SettingsManager.Load("appsettings.json", new());
        SettingsManager.Save();
        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
        mainWindow.WindowState = WindowState.Maximized;
#if DEBUG
        // TODO DEBUG
        //mainWindow.ViewModel.Options = new("Debug");
#endif
    }
}
