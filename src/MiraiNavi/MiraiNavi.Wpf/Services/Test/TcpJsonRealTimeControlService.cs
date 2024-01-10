using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MiraiNavi.Shared.Models.Solution;
using MiraiNavi.Shared.Models.Solution.RealTime;
using MiraiNavi.Shared.Serialization;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.Services;

public class TcpJsonRealTimeControlService() : IRealTimeControlService
{
    protected const string _clientPath = "D:\\RemeaMiku study\\course in progress\\Graduation\\projects\\src\\MiraiNavi\\MiraiNavi.Client\\bin\\Debug\\net8.0\\MiraiNavi.Client.exe";

    public bool IsRunning { get; private set; }

    public event EventHandler<EpochData?>? EpochDataReceived;

    public int _epochCount;

    public async Task StartListeningAsync(RealTimeSolutionOptions options, CancellationToken token)
    {
        if (IsRunning)
            throw new InvalidOperationException("It's already started.");
        IsRunning = true;
        try { await options.WaitToStartAsync(token); }
        catch (TaskCanceledException) { return; }
        using var listener = new TcpListener(options.RoverIPEndPoint);
        listener.Start();
        var process = Process.Start(_clientPath);
        try
        {
            await Task.Run(async () =>
            {
                using var client = await listener.AcceptTcpClientAsync(token);
                using var stream = client.GetStream();
                using var reader = new BinaryReader(stream, Encoding.UTF8);
                var jsonOptions = new JsonSerializerOptions();
                jsonOptions.Converters.Add(new UtcTimeJsonConverter());
                while (!token.IsCancellationRequested && !options.NeedStop(_epochCount))
                {
                    var message = reader.ReadString();
                    if (string.IsNullOrEmpty(message))
                        continue;
                    var epochData = JsonSerializer.Deserialize<EpochData>(message, jsonOptions);
                    EpochDataReceived?.Invoke(this, epochData);
                    _epochCount++;
                }
            }, token);
        }
        catch (TaskCanceledException) { }
        catch (OperationCanceledException) { }
        catch (IOException ex)
        {
            if (process.HasExited)
                throw new InvalidOperationException($"解算程序意外退出，返回值为 {process.ExitCode}");
            throw new InvalidOperationException(ex.Message);
        }
        catch (Exception) { throw; }
        finally
        {
            process.Kill();
            listener.Stop();
            _epochCount = 0;
            IsRunning = false;
        }
    }
}
