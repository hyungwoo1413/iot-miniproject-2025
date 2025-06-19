using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Text.Json;

public class MqttToDbService
{
    private readonly IMqttClient _cli;
    private readonly DbWriter _db;

    public MqttToDbService(string connStr)
    {
        _cli = new MqttFactory().CreateMqttClient();
        _db = new DbWriter(connStr);
    }

    public async Task RunAsync(string broker, int port, string topic)
    {
        _cli.ApplicationMessageReceivedAsync += async e =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var doc = JsonDocument.Parse(json);
                // 시간 설정
                var now = TimeZoneInfo.ConvertTimeFromUtc(
                    DateTime.UtcNow,
                    TimeZoneInfo.FindSystemTimeZoneById("Korea Standard Time")
                );
                var data = new SensorData(
                    doc.RootElement.GetProperty("temp").GetDouble(),
                    doc.RootElement.GetProperty("lux").GetDouble(),
                    doc.RootElement.GetProperty("angle").GetDouble(),
                    doc.RootElement.GetProperty("charge").GetDouble(),
                    now
                );
                await _db.InsertAsync(data);
                Console.WriteLine(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"파싱/DB 오류: {ex.Message}");
            }
        };

        var opt = new MqttClientOptionsBuilder()
                  .WithTcpServer(broker, port)
                  .WithClientId("db_writer")
                  .Build();

        await _cli.ConnectAsync(opt);
        await _cli.SubscribeAsync(topic);
        Console.WriteLine($"MQTT 구독 시작: {topic}");
    }
}
