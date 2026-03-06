using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Domain.Entities;

namespace EvChargingOptimizer.Application.Interfaces;

public interface IChargingStationService
{
    Task<IEnumerable<ChargingStation>> GetAllAsync();
    Task<ChargingStation> CreateAsync(CreateChargingStationDto dto);
}