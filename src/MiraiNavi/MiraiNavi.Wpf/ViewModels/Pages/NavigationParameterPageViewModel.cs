using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Models;
using MiraiNavi.WpfApp.Models.Navigation;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class NavigationParameterPageViewModel(IMessenger messenger, IEpochDatasService epochDatasService) : ObservableRecipient(messenger), IRecipient<EpochData>
{
    readonly IEpochDatasService _epochDatasService = epochDatasService;

    [ObservableProperty]
    Pose? _navigationParameters;

    protected override void OnActivated()
    {
        base.OnActivated();
        var newestData = _epochDatasService.Datas.LastOrDefault();
        if (newestData is null)
            NavigationParameters = default;
        else
            Receive(newestData);
    }

    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        NavigationParameters = default;
    }

    public void Receive(EpochData message)
    {
        NavigationParameters = message.Pose;
    }
}
