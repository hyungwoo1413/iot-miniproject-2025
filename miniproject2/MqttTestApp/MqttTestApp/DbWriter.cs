using MySqlConnector;

public class DbWriter
{
    private readonly string _conn;

    public DbWriter(string conn) => _conn = conn;

    public async Task InsertAsync(SensorData data)
    {
        const string sql = @"INSERT INTO sensor_logs (temp, lux, angle, charge, time) 
                             VALUES (@temp, @lux, @angle, @charge, @time);";
        await using var con = new MySqlConnection(_conn);
        await con.OpenAsync();

        await using var cmd = new MySqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@temp", data.Temp);
        cmd.Parameters.AddWithValue("@lux", data.Lux);
        cmd.Parameters.AddWithValue("@angle", data.Angle);
        cmd.Parameters.AddWithValue("@charge", data.Charge);
        cmd.Parameters.AddWithValue("@time", data.Time);

        await cmd.ExecuteNonQueryAsync();
    }
}