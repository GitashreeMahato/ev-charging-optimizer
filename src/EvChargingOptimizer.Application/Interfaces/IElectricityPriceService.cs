using EvChargingOptimizer.Application.DTOs;

namespace EvChargingOptimizer.Application.Interfaces;

public interface IElectricityPriceService
{
    Task<IEnumerable<ElectricityPriceResponseDto>> GetAllAsync();
    Task<ElectricityPriceResponseDto?> GetByIdAsync(int id);
    Task<ElectricityPriceResponseDto> CreateAsync(CreateElectricityPriceDto dto);
    Task<ElectricityPriceResponseDto?> UpdateAsync(int id, CreateElectricityPriceDto dto);
    Task<bool> DeleteAsync(int id);
}
