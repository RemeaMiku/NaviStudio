using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using NaviStudio.Shared.Models;
using NaviStudio.Shared.Serialization;
using NetMQ;
using NetMQ.Sockets;

try
{
    var port = 39831;
    if(args.Length == 1 && !int.TryParse(args[0], out port))
        Console.WriteLine("Invalid port number.");
    using var client = new TcpClient();
    client.Connect(new(IPAddress.Loopback, port));
    using var stream = client.GetStream();
    using var subscriber = new SubscriberSocket();
    subscriber.SubscribeToAnyTopic();
    subscriber.Connect("tcp://localhost:9113");
    var options = new JsonSerializerOptions()
    {
        IgnoreReadOnlyProperties = true,
        Converters = { new UtcTimeJsonConverter() },
    };
    while(true)
    {
        var message = subscriber.ReceiveFrameString();
        var data = JsonSerializer.Deserialize<EpochData>(message, options);
        if(data == null)
        {
            Console.WriteLine("Failed to deserialize message.");
            continue;
        }
        Console.WriteLine($"Received: {data.Result.GeodeticCoord} {data.Result.Velocity} {data.Result.Attitude}");
        data.SatelliteTrackings?.ForEach(t => t.Frequency /= 1E6);
        stream.Write(JsonSerializer.SerializeToUtf8Bytes(data, options));
    }
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine(ex.StackTrace);
}
