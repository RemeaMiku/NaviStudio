using System.Windows.Controls;
using Syncfusion.Windows.Tools.Controls;

namespace NaviStudio.WpfApp.Common.Helpers;

public static class DockingWindowHandler
{
    #region Public Methods

    public static void RestoreDockState(ContentControl contentControl)
    {
        SetViewModelIsActive(contentControl, true);
        if (_dockStatesOnClosed.TryGetValue(contentControl, out var dockState))
            DockingManager.SetState(contentControl, dockState);
    }

    public static void SaveDockState(ContentControl contentControl)
    {
        if (contentControl.Content is null)
            return;
        _dockStatesOnClosed[contentControl] = DockingManager.GetState(contentControl);
    }

    public static void SetViewModelIsActive(ContentControl contentControl, bool isActive)
    {
        var viewModel = contentControl.Content?.GetType()?.GetProperty("ViewModel")?.GetValue(contentControl.Content);
        viewModel?.GetType()?.GetProperty("IsActive")?.SetValue(viewModel, isActive);
    }

    #endregion Public Methods

    #region Private Fields

    static readonly Dictionary<ContentControl, DockState> _dockStatesOnClosed = [];

    #endregion Private Fields
}
