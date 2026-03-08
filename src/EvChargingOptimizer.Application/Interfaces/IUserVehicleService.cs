using EvChargingOptimizer.Application.DTOs;

namespace EvChargingOptimizer.Application.Interfaces;

public interface IUserVehicleService
{
    Task<IEnumerable<UserVehicleResponseDto>> GetAllAsync();
    Task<UserVehicleResponseDto?> GetByIdAsync(int id);
    Task<UserVehicleResponseDto> CreateAsync(CreateUserVehicleDto dto);
    Task<UserVehicleResponseDto?> UpdateAsync(int id, CreateUserVehicleDto dto);
    Task<bool> DeleteAsync(int id);
}
