using Microsoft.AspNetCore.Mvc;
using SolarTest.Services;

namespace SolarTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensorsController : ControllerBase
{
    private readonly SensorDataService _service;

    public SensorsController(SensorDataService service)
    {
        _service = service;
    }

    /// <summary>
    /// 타입별 데이터 조회
    /// </summary>
    [HttpGet("{type}")]
    public async Task<IActionResult> GetByType(string type)
    {
        var data = await _service.GetSensorDataAsync(type);
        return Ok(data);
    }

    /// <summary>
    /// 타입별 실시간 데이터 조회
    /// </summary>
    [HttpGet("latest/{type}")]
    public async Task<IActionResult> GetLatest(string type)
    {
        var data = await _service.GetLatestAsync(type);
        if (data == null)
            return NotFound($"센서 '{type}'의 최신 데이터를 찾을 수 없습니다.");

        return Ok(data);
    }

    /// <summary>
    /// 실시간 전체 데이터 조회
    /// </summary>
    [HttpGet("latest/all")]
    public async Task<IActionResult> GetLatestAll()
    {
        var data = await _service.GetLatestAllAsync();
        return Ok(data);
    }


    [HttpGet("latest/values")]
    public async Task<IActionResult> GetLatestValuesOnly()
    {
        var data = await _service.GetLatestValuesOnlyAsync();
        return Ok(data);
    }

}
