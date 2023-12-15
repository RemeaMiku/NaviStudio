using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MiraiNavi.WpfApp.Views.Pages;
using Syncfusion.Windows.Tools.Controls;

namespace MiraiNavi.WpfApp.Common.Helpers;

public static class DockingWindowHandler
{
    static readonly FrozenSet<Type> _documentTypes = new HashSet<Type>() { typeof(MapPage), typeof(StartPage) }.ToFrozenSet();
    static readonly Dictionary<ContentControl, DockState> _dockStatesOnClosed = [];

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
        if (_documentTypes.Contains(contentControl.Content.GetType()))
            _dockStatesOnClosed[contentControl] = DockState.Document;
        else
            _dockStatesOnClosed[contentControl] = DockingManager.GetState(contentControl);
    }

    public static void SetViewModelIsActive(ContentControl contentControl, bool isActive)
    {
        var viewModel = contentControl.Content?.GetType()?.GetProperty("ViewModel")?.GetValue(contentControl.Content);
        viewModel?.GetType()?.GetProperty("IsActive")?.SetValue(viewModel, isActive);
    }
}
