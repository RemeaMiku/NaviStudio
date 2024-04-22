using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using NaviStudio.WpfApp.Services.Contracts;
using Wpf.Ui.Controls;

namespace NaviStudio.WpfApp.Services;

public class DialogService : IDialogService
{
    protected Dialog? _dialog;
    protected TaskCompletionSource<bool>? _tcs;
    protected bool _autoHide;

    public void RegisteDialog(ContentControl control)
    {
        _dialog = control is Dialog dialog ? dialog : throw new InvalidOperationException();
        _dialog.ButtonLeftClick += (_, _) =>
        {
            _tcs?.SetResult(true);
            if(_autoHide)
                _dialog.Hide();
        };
        _dialog.ButtonRightClick += (_, _) =>
        {
            _tcs?.SetResult(false);
            if(_autoHide)
                _dialog.Hide();
        };
    }

    protected void ThrowIfNotRegistered()
    {
        if(_dialog is null)
            throw new InvalidOperationException();
    }

    public void Hide()
    {
        ThrowIfNotRegistered();
        _dialog?.Hide();
    }

    public async Task<bool> ShowAsync(string? title = null, bool isConfirmRequired = true, bool autoHide = true)
    {
        ThrowIfNotRegistered();
        _dialog!.ButtonLeftVisibility = Visibility.Visible;
        _dialog.ButtonRightVisibility = isConfirmRequired ? Visibility.Visible : Visibility.Collapsed;
        if(title is not null)
            _dialog.Title = title;
        _autoHide = autoHide;
        _tcs = new();
        _dialog.Show();
        await _tcs.Task;
        return _tcs.Task.Result;
    }

    public async Task<bool> ShowMessageAsync(string message, string? title = null, bool isConfirmRequired = true, bool autoHide = true)
    {
        ThrowIfNotRegistered();
        _dialog!.ButtonLeftVisibility = Visibility.Visible;
        _dialog.ButtonRightVisibility = isConfirmRequired ? Visibility.Visible : Visibility.Collapsed;
        _autoHide = autoHide;
        _tcs = new();
        _dialog.Show(title ?? _dialog.Title, message);
        await _tcs.Task;
        return _tcs.Task.Result;
    }
}