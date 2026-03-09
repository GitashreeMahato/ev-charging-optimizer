using EvChargingOptimizer.Application.DTOs;

namespace EvChargingOptimizer.Application.Interfaces;

public interface IChargingSessionService
{
    Task<IEnumerable<ChargingSessionResponseDto>> GetAllAsync();
    Task<ChargingSessionResponseDto?> GetByIdAsync(int id);
    Task<ChargingSessionResponseDto> CreateAsync(CreateChargingSessionDto dto);
    Task<ChargingSessionResponseDto?> UpdateAsync(int id, CreateChargingSessionDto dto);
    Task<bool> DeleteAsync(int id);
}
