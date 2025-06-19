namespace SolarTest.Models;

public class SensorData
{
    public int Id { get; set; }
    public string DeviceId { get; set; } = "";
    public string SensorType { get; set; } = "";
    public double Value { get; set; }
    public DateTime Time { get; set; }
}

