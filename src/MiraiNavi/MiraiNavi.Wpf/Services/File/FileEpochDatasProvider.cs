using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MiraiNavi.WpfApp.Models.Navigation;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.Services.File;

public class FileEpochDatasProvider(string filePath, IEpochDatasService epochDatasService) : IRealTimeControlService
{
    readonly IEpochDatasService _epochDatasService = epochDatasService;
    readonly string _filePath = filePath;
    string[]? _titles;
    Task? _task;
    readonly CancellationTokenSource _tokenSource = new();

    public bool IsRunning => _task is not null && !_task.IsCompleted;

    public void Pause()
    {
        throw new NotImplementedException();
    }

    //public static EpochData ParseLine(Dictionary<string, string> titleValueDictionary)
    //{

    //}

    public void Start()
    {
        if (IsRunning)
            throw new InvalidOperationException();
        using var stream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new StreamReader(stream);
        _titles = reader.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        reader.ReadLine();
        _task = Task.Run(() =>
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line is null)
                    break;
                var titleValueDictionary = new Dictionary<string, string>();
                var values = line.Trim().Split("  ", StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < _titles?.Length; i++)
                    titleValueDictionary.Add(_titles[i], values[i].Trim());
                Debugger.Break();
            }
        }, _tokenSource.Token);

    }

    public void Stop()
    {
        throw new NotImplementedException();
    }
}
