using System.Diagnostics;
using System.Net.Sockets;
using System.Text.Json;
using MiraiNavi.Shared.Models;
using NaviSharp.SpatialReference;

using var listener = new TcpListener(RealTimeControlOptions.DefaultIPEndPoint);
listener.Start();
Console.WriteLine("Listening");
var process = Process.Start("D:\\RemeaMiku study\\course in progress\\Graduation\\projects\\src\\MiraiNavi\\MiraiNavi.Client\\bin\\Debug\\net8.0\\MiraiNavi.Client.exe");
using var client = listener.AcceptTcpClient();
Console.WriteLine("Listener Connected");
using var stream = client.GetStream();
using var reader = new StreamReader(stream);
while (true)
{
    if (stream.DataAvailable)
    {
        var message = reader.ReadLine();
        if (message is null)
            continue;
        Console.WriteLine(message);
        if (message == "END")
            break;
    }
}
process.Kill();
listener.Stop();