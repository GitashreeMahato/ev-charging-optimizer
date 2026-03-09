using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Domain.Entities;

namespace EvChargingOptimizer.Application.Interfaces;

public interface IChargingStationService
{
    Task<IEnumerable<ChargingStationResponseDto>> GetAllAsync();
    Task<ChargingStationResponseDto?> GetByIdAsync(int id);
    Task<ChargingStationResponseDto> CreateAsync(CreateChargingStationDto dto);
    Task<ChargingStationResponseDto?> UpdateAsync(int id, CreateChargingStationDto dto);
    Task<bool> DeleteAsync(int id);

}