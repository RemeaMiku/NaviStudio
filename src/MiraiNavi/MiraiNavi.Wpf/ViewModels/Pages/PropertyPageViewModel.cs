using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.Shared.Models.Solution;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class PropertyPageViewModel : ObservableRecipient, IRecipient<EpochData>
{
    [ObservableProperty]
    EpochData? _epochData;

    public void Receive(EpochData? message)
    {
        EpochData = message;
    }

    protected override void OnActivated()
    {
        Messenger.Register(this, MapPageViewModel.Title);
    }
}
