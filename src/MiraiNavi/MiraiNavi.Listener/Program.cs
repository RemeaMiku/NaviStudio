﻿//using var listener = new TcpListener(RealTimeControlOptions.DefaultIPEndPoint);
//listener.Start();
//Console.WriteLine("Listening");
//var process = Process.Start("D:\\RemeaMiku study\\course in progress\\Graduation\\projects\\src\\MiraiNavi\\MiraiNavi.Client\\bin\\Debug\\net8.0\\MiraiNavi.Client.exe");
//using var client = listener.AcceptTcpClient();
//Console.WriteLine("Listener Connected");
//using var stream = client.GetStream();
//using var reader = new BinaryReader(stream, Encoding.UTF8);
//var options = new JsonSerializerOptions()
//{
//    IgnoreReadOnlyProperties = true,
//    WriteIndented = true,
//};
//options.Converters.Add(new UtcTimeJsonConverter());
//options.Converters.Add(new Vector3JsonConverter());
//while (true)
//{
//    var message = reader.ReadString();
//    if (message is null)
//        continue;
//    var epochData = JsonSerializer.Deserialize<EpochData>(message, options);
//    Console.WriteLine($"Deserialized: {epochData!.TimeStamp:yyyy/MM/dd HH:mm:ss.fff}");
//}

new List<double>() { 1, 2, 3 }.TakeLast(4);