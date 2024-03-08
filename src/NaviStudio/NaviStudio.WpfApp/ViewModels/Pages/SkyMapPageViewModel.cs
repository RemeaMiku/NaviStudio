using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using NaviStudio.Shared.Models.Satellites;
using NaviStudio.WpfApp.Services.Contracts;
using NaviStudio.WpfApp.ViewModels.Base;

namespace NaviStudio.WpfApp.ViewModels.Pages;

public partial class SkyMapPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableSingleEpochDataRecipient(messenger, epochDatasService)
{
    #region Public Fields

    public const string Title = "卫星天空图";
    public const string MenuItemHeader = $"{Title}(_S)";

    #endregion Public Fields

    #region Public Properties

    public IEnumerable<SatelliteSkyPosition>? EnabledPositions => _positions?.Where(p => p.Elevation.Degrees >= MinElevation && _enabledSystems.Contains(p.Satellite.System));

    #endregion Public Properties

    #region Protected Methods

    public override void Update(EpochData message)
    {
        if(message.SatelliteSkyPositions is null)
        {
            Reset();
            return;
        }
        _positions = message.SatelliteSkyPositions;
        OnPropertyChanged(nameof(EnabledPositions));
    }

    public override void Reset()
    {
        _positions = default;
        OnPropertyChanged(nameof(EnabledPositions));
    }

    #endregion Protected Methods

    #region Private Fields

    readonly HashSet<SatelliteSystems> _enabledSystems = new(Enum.GetValues<SatelliteSystems>());

    List<SatelliteSkyPosition>? _positions = default;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EnabledPositions))]
    double _minElevation = 5;

    #endregion Private Fields

    #region Private Methods

    [RelayCommand]
    void EnableOrDisableSystem(SatelliteSystems systems)
    {
        if(!_enabledSystems.Remove(systems))
            _enabledSystems.Add(systems);
        OnPropertyChanged(nameof(EnabledPositions));
    }

    #endregion Private Methods
}
