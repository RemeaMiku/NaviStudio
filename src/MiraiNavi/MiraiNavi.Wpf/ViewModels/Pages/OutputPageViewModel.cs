using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MiraiNavi.WpfApp.Models;
using Wpf.Ui.Controls;

namespace MiraiNavi.WpfApp.ViewModels.Pages;

public partial class OutputPageViewModel : ObservableObject, IRecipient<Output>
{
    public static string Title => "输出";

    readonly IMessenger _messenger;

    const string _allSenders = "全部";

    [ObservableProperty]
    string _selectedSender = _allSenders;

    public bool ShowError => _severityFlags[InfoBarSeverity.Error];

    public int ErrorCount => _severityCounts[InfoBarSeverity.Error];

    public bool ShowWarning => _severityFlags[InfoBarSeverity.Warning];

    public int WarningCount => _severityCounts[InfoBarSeverity.Warning];

    public bool ShowInformational => _severityFlags[InfoBarSeverity.Informational];

    public int InformationalCount => _severityCounts[InfoBarSeverity.Informational];

    public bool ShowSuccess => _severityFlags[InfoBarSeverity.Success];

    public int SuccessCount => _severityCounts[InfoBarSeverity.Success];

    readonly Dictionary<InfoBarSeverity, bool> _severityFlags = [];

    readonly Dictionary<InfoBarSeverity, int> _severityCounts = [];

    public OutputPageViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        _messenger.RegisterAll(this);
        OutputsView = CollectionViewSource.GetDefaultView(Outputs);
        OutputsView.Filter = OutputsViewFilter;
        _severityFlags.Add(InfoBarSeverity.Error, true);
        _severityFlags.Add(InfoBarSeverity.Warning, true);
        _severityFlags.Add(InfoBarSeverity.Informational, true);
        _severityFlags.Add(InfoBarSeverity.Success, true);
        _severityCounts.Add(InfoBarSeverity.Error, default);
        _severityCounts.Add(InfoBarSeverity.Warning, default);
        _severityCounts.Add(InfoBarSeverity.Informational, default);
        _severityCounts.Add(InfoBarSeverity.Success, default);
    }

    bool OutputsViewFilter(object item)
    {
        var output = (Output)item;
        var severityFlag = _severityFlags[output.Severity];
        var senderFlag = SelectedSender == _allSenders || output.SenderName == SelectedSender;
        var accepted = severityFlag && senderFlag && output.DisplayMessage.Contains(SearchKeyword);
        if (accepted)
            _severityCounts[output.Severity]++;
        return accepted;
    }

    void Refresh()
    {
        foreach (var severity in _severityCounts.Keys)
            _severityCounts[severity] = default;
        OutputsView.Refresh();
        OnPropertyChanged(string.Empty);
    }

    partial void OnSelectedSenderChanged(string value) => Refresh();

    public ObservableCollection<string> Senders { get; } = [_allSenders];

    public ObservableCollection<Output> Outputs { get; } = [];

    public ICollectionView OutputsView { get; }

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
    void SwitchVisibility(InfoBarSeverity severity)
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
