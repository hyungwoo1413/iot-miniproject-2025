using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        const string broker = "127.0.0.1";
        const int port = 1883;
        const string topic = "solar/data";
        const string connStr = "Server=localhost;Database=iot;User ID=root;Password=12345;";

        var svc = new MqttToDbService(connStr);
        await svc.RunAsync(broker, port, topic);

        Console.WriteLine("엔터를 누르면 종료합니다...");
        Console.ReadLine();
    }
}
