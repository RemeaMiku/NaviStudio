using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NaviStudio.Shared.Models.Options;
using NaviStudio.Shared.Serialization;
using NaviStudio.WpfApp.Services.Contracts;


namespace NaviStudio.WpfApp.Services;

public class TcpJsonRealTimeService() : IRealTimeService
{
    #region Public Events

    public event EventHandler<EpochData?>? EpochDataReceived;

    #endregion Public Events

    #region Public Properties

    public bool IsRunning { get; private set; }

    #endregion Public Properties

    #region Public Methods

    public async Task StartAsync(RealTimeOptions options, CancellationToken token)
    {
        if(IsRunning)
            throw new InvalidOperationException("It's already started.");
        IsRunning = true;
        try
        {
            using var listener = new TcpListener(App.Current.SettingsManager.Settings.SolutionSettings.EpochDataTcpOptions.ToIPEndPoint());
            listener.Start();
            var process = Process.Start(_clientPath);
            using var client = await listener.AcceptTcpClientAsync(token);
            using var tcpStream = client.GetStream();
            var IsOutputRequested = !string.IsNullOrEmpty(options.OutputFolder);
            if(IsOutputRequested)
                Directory.CreateDirectory(options.OutputFolder);
            using var fileStream = IsOutputRequested ? new FileStream(Path.Combine(options.OutputFolder, $"{UtcTime.Now:yyMMddHHmmss}.edjson"), FileMode.Create, FileAccess.Write, FileShare.Read) : default;
            using var writer = fileStream is null ? default : new Utf8JsonWriter(fileStream);
            writer?.WriteStartArray();
            using var reader = new BinaryReader(tcpStream, Encoding.UTF8);
            var jsonOptions = new JsonSerializerOptions()
            {
                IgnoreReadOnlyProperties = true
            };
            jsonOptions.Converters.Add(new UtcTimeJsonConverter());
            await Task.Run(() =>
            {
                while(!token.IsCancellationRequested)
                {
                    var message = reader.ReadString();
                    if(string.IsNullOrEmpty(message))
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
        catch(TaskCanceledException) { }
        catch(OperationCanceledException) { }
        finally
        {
            IsRunning = false;
        }
    }

    #endregion Public Methods

    #region Private Fields

    const string _clientPath = @"D:\RemeaMiku study\course in progress\Graduation\projects\src\NaviStudio\NaviStudio.Client\bin\Debug\net8.0\MiraiNavi.Client.exe";

    #endregion Private Fields
}
