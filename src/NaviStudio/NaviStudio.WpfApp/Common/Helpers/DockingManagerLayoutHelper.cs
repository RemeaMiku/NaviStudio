using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Syncfusion.Windows.Tools.Controls;

namespace NaviStudio.WpfApp.Common.Helpers;

public static class DockingManagerLayoutHelper
{
    static DockingManager? _dockingManager;

    public const int MaxLayoutCount = 10;

    const string _layoutsFilePath = "layouts.json";

    const string _layoutsStorageDirectory = "layouts";

    const string _defaultLayoutFileName = "$default.xml";

    static Dictionary<string, string>? _layoutNameToFileNameMap;

    public static void Load()
    {
        if(_layoutNameToFileNameMap is null && File.Exists(_layoutsFilePath))
        {
            _layoutNameToFileNameMap = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(_layoutsFilePath));
            if(_layoutNameToFileNameMap?.Count > MaxLayoutCount)
                _layoutNameToFileNameMap = _layoutNameToFileNameMap?.Take(MaxLayoutCount).ToDictionary();
        }
        _layoutNameToFileNameMap ??= [];
    }

    static void ThrowIfNotLoaded()
    {
        if(_layoutNameToFileNameMap is null)
            throw new InvalidOperationException();
    }

    static void ThrowIfNotRegistered()
    {
        if(_dockingManager is null)
            throw new InvalidOperationException();
    }

    public static IEnumerable<string> GetLayoutNames()
    {
        ThrowIfNotLoaded();
        return _layoutNameToFileNameMap!.Keys;
    }

    public static void Register(DockingManager manager)
    {
        _dockingManager = manager;
    }

    public enum SaveResult
    {
        Success,
        ExceedMaxCount,
        AlreadyExists,
    }

    static void SaveDockingManagerLayout(string fileName)
    {
        ThrowIfNotRegistered();
        var filePath = Path.Combine(_layoutsStorageDirectory, fileName);
        Directory.CreateDirectory(_layoutsStorageDirectory);
        _dockingManager!.SaveDockState(filePath);
    }

    public static SaveResult Save(string layoutName)
    {
        ThrowIfNotRegistered();
        ThrowIfNotLoaded();
        ArgumentException.ThrowIfNullOrWhiteSpace(layoutName);
        if(_layoutNameToFileNameMap!.ContainsKey(layoutName))
            return SaveResult.AlreadyExists;
        if(_layoutNameToFileNameMap.Count >= MaxLayoutCount)
            return SaveResult.ExceedMaxCount;
        var builder = new StringBuilder(layoutName);
        foreach(var @char in Path.GetInvalidFileNameChars())
            builder.Replace(@char, '_');
        builder.Replace('$', '_');
        builder.Append(".xml");
        var fileName = builder.ToString();
        _layoutNameToFileNameMap!.Add(layoutName, fileName);
        SaveDockingManagerLayout(fileName);
        File.WriteAllText(_layoutsFilePath, JsonSerializer.Serialize(_layoutNameToFileNameMap));
        return SaveResult.Success;
    }



    public static void SaveDefault()
    {
        ThrowIfNotRegistered();
        SaveDockingManagerLayout(_defaultLayoutFileName);
    }

    static bool ApplyDockingManagerLayout(string fileName)
    {
        var filePath = Path.Combine(_layoutsStorageDirectory, fileName);
        if(!File.Exists(filePath))
            return false;
        _dockingManager!.LoadDockState(filePath);
        return true;
    }

    public static bool ApplyDefault()
    {
        ThrowIfNotRegistered();
        return ApplyDockingManagerLayout(_defaultLayoutFileName);
    }

    public static void Remove(string layoutName)
    {
        ThrowIfNotRegistered();
        ThrowIfNotLoaded();
        if(_layoutNameToFileNameMap!.TryGetValue(layoutName, out var fileName))
        {
            _layoutNameToFileNameMap.Remove(layoutName);
            File.WriteAllText(_layoutsFilePath, JsonSerializer.Serialize(_layoutNameToFileNameMap));
            var filePath = Path.Combine(_layoutsStorageDirectory, fileName);
            if(File.Exists(filePath))
                File.Delete(filePath);
        }
    }

    public static void Replace(string layoutName)
    {
        ThrowIfNotRegistered();
        ThrowIfNotLoaded();
        if(!_layoutNameToFileNameMap!.TryGetValue(layoutName, out var fileName))
            throw new InvalidOperationException();
        SaveDockingManagerLayout(fileName);
    }

    public static bool Apply(string layoutName)
    {
        ThrowIfNotRegistered();
        ThrowIfNotLoaded();
        var fileName = _layoutNameToFileNameMap![layoutName];
        return ApplyDockingManagerLayout(fileName);
    }
}
