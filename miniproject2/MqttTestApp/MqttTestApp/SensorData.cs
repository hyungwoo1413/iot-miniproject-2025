
public class SensorData
{
    public double Temp { get; set; }
    public double Lux { get; set; }
    public double Angle { get; set; }
    public double Charge { get; set; }
    public DateTime Time { get; set; }

    public SensorData(double temp, double lux, double angle, double charge, DateTime time)
    {
        Temp = temp;
        Lux = lux;
        Angle = angle;
        Charge = charge;
        Time = time;
    }

    public override string ToString()
    {
        return $"[{Time:yyyy-MM-dd HH:mm:ss}] " +
               $"Temp: {Temp}, Lux: {Lux}, Angle: {Angle}, Charge: {Charge}";
    }
}
