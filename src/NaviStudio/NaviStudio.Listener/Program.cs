using System.Text;
using NetMQ;
using NetMQ.Sockets;

using var subscriber = new SubscriberSocket();
subscriber.SubscribeToAnyTopic();
subscriber.Connect("tcp://localhost:9112");
while(true)
{
    var msg = subscriber.ReceiveFrameString();
    Console.WriteLine(msg);
}