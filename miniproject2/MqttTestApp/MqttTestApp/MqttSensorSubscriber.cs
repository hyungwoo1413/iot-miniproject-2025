using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Threading.Tasks;

public class MqttSensorSubscriber
{
    private readonly IMqttClient _cli;
    private readonly DbWriter _db;

    public MqttSensorSubscriber(string connStr)
    {
        _cli = new MqttFactory().CreateMqttClient();
        _db = new DbWriter(connStr);
    }

    public async Task StartAsync(string broker, int port)
    {
        _cli.ApplicationMessageReceivedAsync += async e =>
        {
            string topic = e.ApplicationMessage.Topic;  // 예: device/main/temp
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            string[] parts = topic.Split('/');

            if (parts.Length != 3 || parts[0] != "device")
            {
                Console.WriteLine($"[무시] 잘못된 토픽 형식: {topic}");
                return;
            }

            string deviceId = parts[1];
            string sensorType = parts[2];

            if (double.TryParse(payload, out double value))
            {
                var data = new SensorData
                {
                    DeviceId = deviceId,
                    SensorType = sensorType,
                    Value = value,
                    Time = DateTime.Now
                };

                Console.WriteLine($"[수신] {data}");
                await _db.InsertAsync(data);
            }
            else
            {
                Console.WriteLine($"[오류] 숫자 아님 - {topic}: {payload}");
            }

            await Task.CompletedTask;
        };

        var opt = new MqttClientOptionsBuilder()
            .WithTcpServer(broker, port)
            .WithClientId("structured_subscriber")
            .Build();

        await _cli.ConnectAsync(opt);
        await _cli.SubscribeAsync("device/+/+");
        Console.WriteLine("MQTT 구독 시작...");
    }
}
