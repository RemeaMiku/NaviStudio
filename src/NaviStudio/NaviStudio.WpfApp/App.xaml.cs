using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
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
using Syncfusion.Themes.FluentLight.WPF;
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

    public static void RegisterThemes()
    {
        Accent.Apply((Color)ColorConverter.ConvertFromString("#FF39c5bb"));
        SfSkinManager.RegisterThemeSettings("FluentDark", new FluentDarkThemeSettings()
        {
            PrimaryBackground = Accent.PrimaryAccentBrush,
            FontFamily = new("Microsoft YaHei UI"),
            PrimaryForeground = Brushes.White,
        });
        SfSkinManager.RegisterThemeSettings("FluentLight", new FluentLightThemeSettings()
        {
            PrimaryBackground = Accent.PrimaryAccentBrush,
            FontFamily = new("Microsoft YaHei UI"),
            PrimaryForeground = Brushes.Black,
        });
    }

    public void ApplyBackground()
    {
        foreach(var window in Windows)
        {
            if(window is not UiWindow uiWindow)
                continue;
            if(!SettingsManager.Settings.AppearanceSettings.EnableAcrylic)
            {
                uiWindow.WindowBackdropType = BackgroundType.None;
                return;
            }
            try
            {
                uiWindow.WindowBackdropType = BackgroundType.Acrylic;
            }
            catch(Exception)
            {
                uiWindow.WindowBackdropType = BackgroundType.None;
                SettingsManager.Settings.AppearanceSettings.EnableAcrylic = false;
            }
        }
    }

    static void RegisterKeys()
    {
        SyncfusionLicenseProvider.RegisterLicense(Keys.Syncfusion);
        BingSatelliteMapProvider.Instance.ClientKey = Keys.BingMap;
    }

    public void ApplyTheme()
    {
        var settings = SettingsManager.Settings.AppearanceSettings;
        var themeMode = settings.ThemeMode;
        var backgroundType = settings.EnableAcrylic ? BackgroundType.Acrylic : BackgroundType.None;
        switch(themeMode)
        {
            case ThemeModes.Light:
                Wpf.Ui.Appearance.Theme.Apply(ThemeType.Light, backgroundType, false);
                break;
            case ThemeModes.Dark:
                Wpf.Ui.Appearance.Theme.Apply(ThemeType.Dark, backgroundType, false);
                break;
            case ThemeModes.System:
                foreach(var window in Windows)
                    if(window is UiWindow uiWindow)
                        Watcher.Watch(uiWindow, backgroundType, false);
                break;
            default:
                break;
        }
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
#if RELEASE
        DispatcherUnhandledException += OnAppDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
        TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobservedTaskException;
#endif
        SetGMap();
        RegisterKeys();
        RegisterThemes();
        new SplashScreen("Assets/splash-screen.png").Show(true);
        SettingsManager.Load();
        SettingsManager.Save();
        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
        mainWindow.WindowState = WindowState.Maximized;
        if(e.Args.Length == 1)
        {
            if(Path.Exists(e.Args[0]))
            {
                Services.GetRequiredService<RealTimeOptionsPageViewModel>().ReadCommand.Execute(e.Args[0]);
            }
        }
    }

    #region Global Exception Handling

    private void OnTaskSchedulerUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        ExceptionWindow.Show(e.Exception);
        e.SetObserved();
        Shutdown();
    }

    private void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if(e.ExceptionObject is Exception exception)
            ExceptionWindow.Show(exception);
        else if(e.ExceptionObject is null)
            ExceptionWindow.Show(string.Empty);
        else
            ExceptionWindow.Show(e.ExceptionObject.ToString() ?? string.Empty);
        if(e.IsTerminating)
            Shutdown();
    }

    private void OnAppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        try
        {
            ExceptionWindow.Show(e.Exception);
            e.Handled = true;
        }
        catch(Exception ex)
        {
            ExceptionWindow.Show(ex);
            Shutdown();
        }
    }
    #endregion
}
