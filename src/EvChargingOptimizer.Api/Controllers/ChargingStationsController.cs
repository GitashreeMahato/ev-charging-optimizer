using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EvChargingOptimizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChargingStationsController : ControllerBase
{
    private readonly IChargingStationService _service;

    public ChargingStationsController(IChargingStationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stations = await _service.GetAllAsync();
        return Ok(stations);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateChargingStationDto dto)
    {
        var station = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetAll), new { id = station.Id, station });

    }
}