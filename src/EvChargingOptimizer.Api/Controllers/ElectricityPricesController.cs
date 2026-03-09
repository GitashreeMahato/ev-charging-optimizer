using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EvChargingOptimizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ElectricityPricesController : ControllerBase
{
    private readonly IElectricityPriceService _service;

    public ElectricityPricesController(IElectricityPriceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var prices = await _service.GetAllAsync();
        return Ok(prices);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var price = await _service.GetByIdAsync(id);
        if (price == null) return NotFound();
        return Ok(price);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateElectricityPriceDto dto)
    {
        var price = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = price.Id }, price);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateElectricityPriceDto dto)
    {
        var price = await _service.UpdateAsync(id, dto);
        if (price == null) return NotFound();
        return Ok(price);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
