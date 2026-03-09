using EvChargingOptimizer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EvChargingOptimizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExternalPricesController : ControllerBase
{
    private readonly IExternalPriceService _service;

    public ExternalPricesController(IExternalPriceService service)
    {
        _service = service;
    }

    [HttpGet("fetch-today")]
    public async Task<IActionResult> FetchToday()
    {
        var prices = await _service.FetchTodayPricesAsync();
        return Ok(prices);
    }
}