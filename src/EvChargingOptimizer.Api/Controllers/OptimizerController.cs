using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EvChargingOptimizer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OptimizerController : ControllerBase
{
    private readonly IOptimizerService _service;

    public OptimizerController(IOptimizerService service)
    {
        _service = service;
    }
    [HttpPost("optimize")]

    public async Task<IActionResult> Optimize(OptimizeChargingRequestDto request)
    {
        try
        {
            var result = await _service.OptimizeAsync(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {

            return BadRequest(ex.Message);
        }
    }
}