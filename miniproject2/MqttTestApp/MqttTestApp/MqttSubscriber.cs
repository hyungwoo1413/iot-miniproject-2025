using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System;
using System.Text;
using System.Threading.Tasks;

public class MqttSubscriber
{
    private readonly IMqttClient mqttClient;

    public MqttSubscriber()
    {
        var factory = new MqttFactory();
        mqttClient = factory.CreateMqttClient();
    }

    public async Task StartAsync(string broker, int port, string topic)
    {
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            Console.WriteLine($"({e.ApplicationMessage.Topic}) {payload}");
            return Task.CompletedTask;
        };

        var options = new MqttClientOptionsBuilder()
            .WithTcpServer(broker, port)
            .WithClientId("mqtt_subscriber_client")
            .Build();

        await mqttClient.ConnectAsync(options);
        Console.WriteLine("Subscirber 연결 완료");

        await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
            .WithTopic(topic)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .Build());

        Console.WriteLine($"{topic} 구독 완료");
    }
}
