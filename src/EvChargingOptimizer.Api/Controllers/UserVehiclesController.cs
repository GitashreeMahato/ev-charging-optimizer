using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EvChargingOptimizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserVehiclesController : ControllerBase
{
    private readonly IUserVehicleService _service;

    public UserVehiclesController(IUserVehicleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vehicles = await _service.GetAllAsync();
        return Ok(vehicles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vehicle = await _service.GetByIdAsync(id);
        if (vehicle == null) return NotFound();
        return Ok(vehicle);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserVehicleDto dto)
    {
        var vehicle = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateUserVehicleDto dto)
    {
        var vehicle = await _service.UpdateAsync(id, dto);
        if (vehicle == null) return NotFound();
        return Ok(vehicle);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}