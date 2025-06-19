class Program
{
    static async Task Main()
    {
        const string broker = "127.0.0.1";
        const int port = 1883;
        const string connStr = "Server=localhost;Database=iot;User ID=root;Password=12345;";

        var subscriber = new MqttSensorSubscriber(connStr);
        await subscriber.StartAsync(broker, port);

        Console.WriteLine("엔터를 누르면 종료합니다...");
        Console.ReadLine();
    }
}
