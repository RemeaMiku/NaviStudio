using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MiraiNavi.WpfApp.Common.Messaging;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class PropertyPageViewModel : ObservableRecipient, IRecipient<ValueChangedMessage<EpochData?>>
{
    #region Public Fields

    public const string Title = "选中点属性";
    public const string MenuItemHeader = $"{Title}(_P)";

    #endregion Public Fields

    #region Public Methods

    public void Receive(ValueChangedMessage<EpochData?> message)
    {
        EpochData = message.Value;
    }

    #endregion Public Methods

    #region Protected Methods

    protected override void OnActivated()
    {
        if (!Messenger.IsRegistered<PropertyPageViewModel>(this))
            Messenger.Register(this, MessageTokens.MapPageToPropertyPage);
    }

    protected override void OnDeactivated() { }

    #endregion Protected Methods

    #region Private Fields

    [ObservableProperty]
    EpochData? _epochData;

    #endregion Private Fields
}
