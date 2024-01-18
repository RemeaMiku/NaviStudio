using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Models.Output;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class OutputPageViewModel : ObservableObject, IRecipient<Output>
{
    public const string Title = "输出";
    public const string MenuItemHeader = $"{Title}(_O)";

    readonly IMessenger _messenger;

    const string _allSenders = "全部";

    [ObservableProperty]
    string _selectedSender = _allSenders;

    public bool ShowError => _severityFlags[OutputType.Error];

    public int ErrorCount => _severityCounts[OutputType.Error];

    public bool ShowWarning => _severityFlags[OutputType.Warning];

    public int WarningCount => _severityCounts[OutputType.Warning];

    public bool ShowInformational => _severityFlags[OutputType.Info];

    public int InformationalCount => _severityCounts[OutputType.Info];

    readonly Dictionary<OutputType, bool> _severityFlags = [];

    readonly Dictionary<OutputType, int> _severityCounts = [];

    public OutputPageViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        _messenger.RegisterAll(this);
        _severityFlags.Add(OutputType.Error, true);
        _severityFlags.Add(OutputType.Warning, true);
        _severityFlags.Add(OutputType.Info, true);
        _severityCounts.Add(OutputType.Error, default);
        _severityCounts.Add(OutputType.Warning, default);
        _severityCounts.Add(OutputType.Info, default);
    }

    public bool OutputsViewFilter(object item)
    {
        var output = (Output)item;
        var severityFlag = _severityFlags[output.Type];
        var senderFlag = SelectedSender == _allSenders || output.SenderName == SelectedSender;
        var accepted = severityFlag && senderFlag && output.DisplayMessage.Contains(SearchKeyword);
        if (accepted)
            _severityCounts[output.Type]++;
        return accepted;
    }

    void Refresh()
    {
        foreach (var severity in _severityCounts.Keys)
            _severityCounts[severity] = default;
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
    void SwitchVisibility(OutputType severity)
    {
        _severityFlags[severity] = !_severityFlags[severity];
        Refresh();
        ClearFilterCommand.NotifyCanExecuteChanged();
    }

    public bool Filtered => _severityFlags.Values.Any(f => !f);

    [RelayCommand(CanExecute = nameof(Filtered))]
    void ClearFilter()
    {
        foreach (var severity in _severityFlags.Keys)
            _severityFlags[severity] = true;
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
