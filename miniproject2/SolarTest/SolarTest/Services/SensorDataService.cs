using MySqlConnector;
using SolarTest.Models;

namespace SolarTest.Services;

public class SensorDataService
{
    private readonly string _conn;

    public SensorDataService(IConfiguration config)
    {
        _conn = config.GetConnectionString("Default")!;
    }

    // 타입별 데이터
    public async Task<List<SensorData>> GetSensorDataAsync(string sensorType)
    {
        var result = new List<SensorData>();
        const string sql = @"SELECT * FROM (
                                SELECT * FROM sensor_data
                                WHERE sensor_type = @type
                                ORDER BY id DESC
                                LIMIT 100
                             ) AS recent
                             ORDER BY id ASC";

        await using var con = new MySqlConnection(_conn);
        await con.OpenAsync();

        await using var cmd = new MySqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@type", sensorType);

        var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new SensorData
            {
                Id = reader.GetInt32("id"),
                DeviceId = reader.GetString("device_id"),
                SensorType = reader.GetString("sensor_type"),
                Value = reader.GetDouble("value"),
                Time = reader.GetDateTime("time")
            });
        }

        return result;
    }

    // 타입별 실시간 데이터
    public async Task<SensorData?> GetLatestAsync(string sensorType)
    {
        const string sql = @"SELECT * FROM sensor_data 
                             WHERE sensor_type = @type 
                             ORDER BY time DESC, id DESC
                             LIMIT 1";

        await using var con = new MySqlConnection(_conn);
        await con.OpenAsync();

        await using var cmd = new MySqlCommand(sql, con);
        cmd.Parameters.AddWithValue("@type", sensorType);

        var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new SensorData
            {
                Id = reader.GetInt32("id"),
                DeviceId = reader.GetString("device_id"),
                SensorType = reader.GetString("sensor_type"),
                Value = reader.GetDouble("value"),
                Time = reader.GetDateTime("time")
            };
        }

        return null;
    }

    // 실시간 전체 데이터
    public async Task<List<SensorData>> GetLatestAllAsync()
    {
        const string sql = @"SELECT t.*
                             FROM sensor_data t
                             INNER JOIN (
                                SELECT sensor_type, MAX(id) AS max_id
                                FROM sensor_data
                                GROUP BY sensor_type
                             ) latest
                             ON t.sensor_type = latest.sensor_type AND t.id = latest.max_id
                             ORDER BY t.sensor_type;";

        var result = new List<SensorData>();

        await using var con = new MySqlConnection(_conn);
        await con.OpenAsync();

        await using var cmd = new MySqlCommand(sql, con);
        var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result.Add(new SensorData
            {
                Id = reader.GetInt32("id"),
                DeviceId = reader.GetString("device_id"),
                SensorType = reader.GetString("sensor_type"),
                Value = reader.GetDouble("value"),
                Time = reader.GetDateTime("time")
            });
        }

        return result;
    }



    // 실시간 데이터 값만 출력
    public async Task<Dictionary<string, double>> GetLatestValuesOnlyAsync()
    {
        const string sql = @"SELECT t.*
                             FROM sensor_data t
                             INNER JOIN (
                                SELECT sensor_type, MAX(id) AS max_id
                                FROM sensor_data
                                GROUP BY sensor_type
                             ) latest
                             ON t.sensor_type = latest.sensor_type AND t.id = latest.max_id";

        var result = new Dictionary<string, double>();

        await using var con = new MySqlConnection(_conn);
        await con.OpenAsync();

        await using var cmd = new MySqlCommand(sql, con);
        var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            string sensorType = reader.GetString("sensor_type");
            double value = reader.GetDouble("value");

            result[sensorType] = value;
        }

        return result;
    }

}
