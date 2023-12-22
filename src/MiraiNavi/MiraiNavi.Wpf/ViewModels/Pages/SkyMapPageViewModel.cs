using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.Shared.Models.Satellites;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Services.Contracts;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

//TODO 高度角和系统筛选器
public partial class SkyMapPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableNotificationEpochDataRecipient(messenger, epochDatasService)
{
    public static string Title => "卫星天空图";

    public IEnumerable<SatelliteSkyPosition> EnabledPositions => _positions.Where(p => p.Elevation >= MinElevation && _enabledSystems.Contains(p.Satellite.System));

    IEnumerable<SatelliteSkyPosition> _positions = Enumerable.Empty<SatelliteSkyPosition>();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EnabledPositions))]
    double _minElevation = 0;

    readonly List<SatelliteSystems> _enabledSystems = new(Enum.GetValues<SatelliteSystems>());

    public override void Receive(EpochData message)
    {
        if (message.SatelliteSkyPositions is null)
        {
            Messenger.Send(new Output(UtcTime.Now, Title, InfoBarSeverity.Warning, "卫星坐标数据缺失"));
            Reset();
            return;
        }
        _positions = message.SatelliteSkyPositions;
        OnPropertyChanged(nameof(EnabledPositions));
    }

    protected override void Reset()
    {
        _positions = Enumerable.Empty<SatelliteSkyPosition>();
        OnPropertyChanged(nameof(EnabledPositions));
    }
}
