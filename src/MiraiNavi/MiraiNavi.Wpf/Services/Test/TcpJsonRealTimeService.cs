using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MiraiNavi.Shared.Models.Options;
using MiraiNavi.Shared.Serialization;
using MiraiNavi.WpfApp.Common.Helpers;
using MiraiNavi.WpfApp.Services.Contracts;

namespace MiraiNavi.WpfApp.Services;

public class TcpJsonRealTimeService() : IRealTimeService
{
    const string _clientPath = "D:\\RemeaMiku study\\course in progress\\Graduation\\projects\\src\\MiraiNavi\\MiraiNavi.Client\\bin\\Debug\\net8.0\\MiraiNavi.Client.exe";

    public bool IsRunning { get; private set; }

    public event EventHandler<EpochData?>? EpochDataReceived;

    public async Task StartAsync(RealTimeOptions options, CancellationToken token)
    {
        if (IsRunning)
            throw new InvalidOperationException("It's already started.");
        IsRunning = true;
        try
        {
            using var listener = new TcpListener(App.Current.SettingsManager.Settings.SolutionSettings.EpochDataTcpOptions.ToIPEndPoint());
            listener.Start();
            var process = Process.Start(_clientPath);
            using var client = await listener.AcceptTcpClientAsync(token);
            using var tcpStream = client.GetStream();
            using var fileStream = string.IsNullOrEmpty(options.OutputFolder) ? default : new FileStream(Path.Combine(options.OutputFolder, $"{UtcTime.Now:yyMMddHHmmss}.mnedf"), FileMode.Create, FileAccess.Write, FileShare.Read);
            using var writer = fileStream is null ? default : new Utf8JsonWriter(fileStream);
            writer?.WriteStartArray();
            using var reader = new BinaryReader(tcpStream, Encoding.UTF8);
            var jsonOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
            };
            jsonOptions.Converters.Add(new UtcTimeJsonConverter());
            await Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    var message = reader.ReadString();
                    if (string.IsNullOrEmpty(message))
                        continue;
                    var epochData = JsonSerializer.Deserialize<EpochData>(message, jsonOptions);
                    writer?.WriteRawValue(JsonSerializer.Serialize(epochData, jsonOptions));
                    writer?.Flush();
                    EpochDataReceived?.Invoke(this, epochData);
                }
            }, token);
            process.Kill();
            listener.Stop();
            writer?.WriteEndArray();
        }
        catch (TaskCanceledException) { }
        catch (OperationCanceledException) { }
        finally
        {
            IsRunning = false;
        }
    }
}
