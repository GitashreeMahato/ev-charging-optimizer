using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EvChargingOptimizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChargingSessionsController : ControllerBase
{
    private readonly IChargingSessionService _service;

    public ChargingSessionsController(IChargingSessionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sessions = await _service.GetAllAsync();
        return Ok(sessions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var session = await _service.GetByIdAsync(id);
        if (session == null) return NotFound();
        return Ok(session);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateChargingSessionDto dto)
    {
        var session = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = session.Id }, session);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateChargingSessionDto dto)
    {
        var session = await _service.UpdateAsync(id, dto);
        if (session == null) return NotFound();
        return Ok(session);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}