using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MiraiNavi.Shared.Models.Solution;
using MiraiNavi.Shared.Serialization;
using MiraiNavi.WpfApp.Common.Helpers;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.Services;

public class TcpJsonRealTimeSolutionService() : IRealTimeSolutionService
{
    protected const string _clientPath = "D:\\RemeaMiku study\\course in progress\\Graduation\\projects\\src\\MiraiNavi\\MiraiNavi.Client\\bin\\Debug\\net8.0\\MiraiNavi.Client.exe";

    public bool IsRunning { get; private set; }

    public event EventHandler<EpochData?>? EpochDataReceived;

    public async Task StartAsync(SolutionOptions options, CancellationToken token)
    {
        if (IsRunning)
            throw new InvalidOperationException("It's already started.");
        IsRunning = true;
        try
        {
            using var listener = new TcpListener(AppSettingsManager.Settings.SolutionSettings.EpochDataEndPoint);
            listener.Start();
            var process = Process.Start(_clientPath);
            using var client = await listener.AcceptTcpClientAsync(token);
            using var stream = client.GetStream();
            using var reader = new BinaryReader(stream, Encoding.UTF8);
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new UtcTimeJsonConverter());
            await Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    var message = reader.ReadString();
                    if (string.IsNullOrEmpty(message))
                        continue;
                    var epochData = JsonSerializer.Deserialize<EpochData>(message, jsonOptions);
                    EpochDataReceived?.Invoke(this, epochData);
                }
            }, token);
            process.Kill();
            listener.Stop();
        }
        catch (TaskCanceledException) { }
        catch (OperationCanceledException) { }
        finally
        {
            IsRunning = false;
        }
    }
}
