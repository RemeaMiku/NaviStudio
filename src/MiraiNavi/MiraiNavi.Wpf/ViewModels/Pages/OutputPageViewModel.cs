using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Common;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class OutputPageViewModel : ObservableObject, IRecipient<Output>
{
    public const string Title = "输出";
    public const string MenuItemHeader = $"{Title}(_O)";

    readonly IMessenger _messenger;

    const string _allSenders = "全部";

    [ObservableProperty]
    string _selectedSender = _allSenders;

    public bool ShowError => _SeverityTypeFlags[SeverityType.Error];

    public int ErrorCount => _SeverityTypeCounts[SeverityType.Error];

    public bool ShowWarning => _SeverityTypeFlags[SeverityType.Warning];

    public int WarningCount => _SeverityTypeCounts[SeverityType.Warning];

    public bool ShowInformational => _SeverityTypeFlags[SeverityType.Info];

    public int InformationalCount => _SeverityTypeCounts[SeverityType.Info];

    readonly Dictionary<SeverityType, bool> _SeverityTypeFlags = [];

    readonly Dictionary<SeverityType, int> _SeverityTypeCounts = [];

    public OutputPageViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        _messenger.RegisterAll(this);
        _SeverityTypeFlags.Add(SeverityType.Error, true);
        _SeverityTypeFlags.Add(SeverityType.Warning, true);
        _SeverityTypeFlags.Add(SeverityType.Info, true);
        _SeverityTypeCounts.Add(SeverityType.Error, default);
        _SeverityTypeCounts.Add(SeverityType.Warning, default);
        _SeverityTypeCounts.Add(SeverityType.Info, default);
    }

    public bool OutputsViewFilter(object item)
    {
        var output = (Output)item;
        var SeverityTypeFlag = _SeverityTypeFlags[output.Type];
        var senderFlag = SelectedSender == _allSenders || output.SenderName == SelectedSender;
        var accepted = SeverityTypeFlag && senderFlag && output.DisplayMessage.Contains(SearchKeyword);
        if (accepted)
            _SeverityTypeCounts[output.Type]++;
        return accepted;
    }

    void Refresh()
    {
        foreach (var SeverityType in _SeverityTypeCounts.Keys)
            _SeverityTypeCounts[SeverityType] = default;
        OutputsView?.Refresh();
        OnPropertyChanged(string.Empty);
    }

    partial void OnSelectedSenderChanged(string value) => Refresh();

    public ObservableCollection<string> Senders { get; } = [_allSenders];

    public ObservableCollection<Output> Outputs { get; } = [];

    public ICollectionView? OutputsView { get; set; }

    public void Receive(Output message)
    {
        App.Current.Dispatcher.Invoke(() =>
        {
            Outputs.Add(message);
            if (!Senders.Contains(message.SenderName))
                Senders.Add(message.SenderName);
            OnPropertyChanged(string.Empty);
        });
        if (KeepBottom)
            ScrollToBottomRequested?.Invoke(this, EventArgs.Empty);
    }

    [RelayCommand]
    void SwitchVisibility(SeverityType SeverityType)
    {
        _SeverityTypeFlags[SeverityType] = !_SeverityTypeFlags[SeverityType];
        Refresh();
        ClearFilterCommand.NotifyCanExecuteChanged();
    }

    public bool Filtered => _SeverityTypeFlags.Values.Any(f => !f);

    [RelayCommand(CanExecute = nameof(Filtered))]
    void ClearFilter()
    {
        foreach (var SeverityType in _SeverityTypeFlags.Keys)
            _SeverityTypeFlags[SeverityType] = true;
        Refresh();
        ClearFilterCommand.NotifyCanExecuteChanged();
    }

    [ObservableProperty]
    string _searchKeyword = string.Empty;

    partial void OnSearchKeywordChanged(string value)
    {
        if (string.IsNullOrEmpty(value))
            Refresh();
    }

    [RelayCommand]
    void Search() => Refresh();

    [ObservableProperty]
    bool _keepBottom;

    public event EventHandler<EventArgs>? ScrollToBottomRequested;

    partial void OnKeepBottomChanged(bool value)
    {
        if (value)
            ScrollToBottomRequested?.Invoke(this, EventArgs.Empty);
    }
}
