using EvChargingOptimizer.Application.DTOs;

namespace EvChargingOptimizer.Application.Interfaces;

public interface IExternalPriceService
{
    Task<IEnumerable<ElectricityPriceResponseDto>> FetchTodayPricesAsync();
}
