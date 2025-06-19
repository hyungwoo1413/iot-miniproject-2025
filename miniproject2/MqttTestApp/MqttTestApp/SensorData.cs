public class SensorData
{
    public string DeviceId { get; set; } = "";
    public string SensorType { get; set; } = "";
    public double Value { get; set; }
    public DateTime Time { get; set; }

    public override string ToString()
    {
        return $"[{Time:yyyy-MM-dd HH:mm:ss}] Device={DeviceId}, Type={SensorType}, Value={Value}";
    }
}
