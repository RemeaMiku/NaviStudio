using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class PropertyPageViewModel : ObservableRecipient, IRecipient<ValueChangedMessage<EpochData?>>
{
    [ObservableProperty]
    object? _epochData;

    public void Receive(ValueChangedMessage<EpochData?> message)
    {
        EpochData = message.Value;
    }

    protected override void OnActivated()
    {
        Messenger.Register(this, MapPageViewModel.Title);
    }
}
