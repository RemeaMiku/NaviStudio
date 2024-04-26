using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NaviStudio.WpfApp.Common.Extensions;
using NaviStudio.WpfApp.Common.Helpers;

namespace NaviStudio.WpfApp.ViewModels.Windows;

partial class MainWindowViewModel
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasLayouts))]
    List<string>? _layoutNames;

    public bool HasLayouts => LayoutNames?.Count > 0;

    string GetInitialNewLayoutName()
    {
        Debug.Assert(LayoutNames is not null);
        var count = 1;
        var name = $"新布局 {count}";
        while(LayoutNames.Contains(name))
            name = $"新布局 {++count}";
        return name;
    }

    [RelayCommand]
    async Task SaveLayoutAsync()
    {
        NewLayoutName = GetInitialNewLayoutName();
        while(true)
        {
            if(!await _dynamicContentDialogService.ShowAsync())
                return;
            if(string.IsNullOrWhiteSpace(NewLayoutName))
            {
                await _messageDialogService.ShowMessageAsync("布局名称不能为空", "保存窗口布局", false);
                continue;
            }
            var result = DockingManagerLayoutHelper.Save(NewLayoutName);
            if(result == DockingManagerLayoutHelper.SaveResult.ExceedMaxCount)
            {
                _snackbarService.ShowError("保存失败", "布局预设数量已达到最大值，请删除其他布局预设后重试。");
                return;
            }
            if(result == DockingManagerLayoutHelper.SaveResult.AlreadyExists)
            {
                if(!await _messageDialogService.ShowMessageAsync($"名为“{NewLayoutName}”的布局已存在，是否要替换？", "保存窗口布局"))
                    continue;
                DockingManagerLayoutHelper.Replace(NewLayoutName);
            }
            _snackbarService.ShowSuccess("保存成功", $"已将当前窗口布局保存为“{NewLayoutName}”。");
            LayoutNames = DockingManagerLayoutHelper.GetLayoutNames().ToList();
            return;
        }
    }

    [RelayCommand]
    async Task ApplyLayout(string layoutName)
    {
        if(!await _messageDialogService.ShowMessageAsync($"是否确定要应用布局“{layoutName}”，并放弃当前窗口布局？", "应用窗口布局"))
            return;
        if(DockingManagerLayoutHelper.Apply(layoutName))
            _snackbarService.ShowSuccess("应用成功", $"已应用布局“{layoutName}”。");
        else
            _snackbarService.ShowError("应用失败", $"未找到名为“{layoutName}”的布局。");
    }

    [RelayCommand]
    async Task ResetLayout()
    {
        if(!await _messageDialogService.ShowMessageAsync($"是否确定要重置布局布局，并放弃当前窗口布局？", "重置窗口布局"))
            return;
        if(DockingManagerLayoutHelper.ApplyDefault())
            _snackbarService.ShowSuccess("重置成功", "已重置窗口布局。");
        else
            _snackbarService.ShowError("重置失败", "未找到默认布局文件。请尝试重启应用。");
    }

    [ObservableProperty]
    string _newLayoutName = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasSelectedLayout))]
    string? _selectedLayoutName;

    public bool HasSelectedLayout => !string.IsNullOrWhiteSpace(SelectedLayoutName);

    [RelayCommand]
    async Task ManageLayouts()
    {
        await _dynamicContentDialogService.ShowAsync("管理窗口布局", false, false);
        _dynamicContentDialogService.Hide();
    }

    [RelayCommand]
    async Task RemoveLayout()
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(SelectedLayoutName));
        if(await _messageDialogService.ShowMessageAsync($"是否确定要删除布局“{SelectedLayoutName}”？", "删除窗口布局"))
        {
            DockingManagerLayoutHelper.Remove(SelectedLayoutName);
            LayoutNames = DockingManagerLayoutHelper.GetLayoutNames().ToList();
        }
    }
}
