using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using NaviStudio.WpfApp.Services.Contracts;
using NaviStudio.WpfApp.Services;
using NaviStudio.WpfApp.ViewModels.Pages;
using NaviStudio.WpfApp.ViewModels.Windows;
using NaviStudio.WpfApp.Views.Pages;
using NaviStudio.WpfApp.Views.Windows;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace NaviStudio.WpfApp.Common.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddViews(this IServiceCollection services)
    {
        return services
            .AddSingleton<RealTimeOptionsPage>()
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
            .AddTransient<AppSettingsWindow>();
    }

    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        return services
            .AddTransient<AppSettingsWindowViewModel>()
            .AddSingleton<RealTimeOptionsPageViewModel>()
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
            .AddTransient<ChartToolWindowViewModel>();
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<ISnackbarService, SnackbarService>()
            .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)
#if DEBUG
            .AddSingleton<IRealTimeService, ZmqRealTimeService>()
#endif
            .AddSingleton<IEpochDatasService, EpochDatasService>()
            .AddSingleton<IGMapRouteDisplayService, GMapRouteDisplayService>();
    }
}
