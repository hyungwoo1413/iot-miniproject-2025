using MySqlConnector;

public class DbWriter
{
    private readonly string _conn;

    public DbWriter(string conn) => _conn = conn;

    public async Task InsertAsync(SensorData data)
    {
        const string sql = @"INSERT INTO sensor_data (device_id, sensor_type, value, time)
                             VALUES (@device, @type, @value, @time);";

        await using var con = new MySqlConnection(_conn);
        await con.OpenAsync();

        await using var cmd = new MySqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@device", data.DeviceId);
        cmd.Parameters.AddWithValue("@type", data.SensorType);
        cmd.Parameters.AddWithValue("@value", data.Value);
        cmd.Parameters.AddWithValue("@time", data.Time);

        await cmd.ExecuteNonQueryAsync();
    }
}
