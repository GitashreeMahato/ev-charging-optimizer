using EvChargingOptimizer.Application.DTOs;
namespace EvChargingOptimizer.Application.Interfaces;

public interface IOptimizerService
{
    Task<OptimizeChargingResponseDto> OptimizeAsync(OptimizeChargingRequestDto request);

}