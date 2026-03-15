using System.Net.Http.Headers;
using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvChargingOptimizer.Api.Controllers;

[Authorize]
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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var station = await _service.GetByIdAsync(id);
        if (station == null) return NotFound();
        return Ok(station);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateChargingStationDto dto)
    {
        var station = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = station.Id }, station);

    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateChargingStationDto dto)
    {
        var station = await _service.UpdateAsync(id, dto);
        if (station == null) return NotFound();
        return Ok(station);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}