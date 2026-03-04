using EvChargingOptimizer.Application.Interfaces;
using EvChargingOptimizer.Domain.Entities;

namespace EvChargingOptimizer.Infrastructure.Services;

public class ChargingStationService : IChargingStationService
{
    public Task<IEnumerable<ChargingStation>> GetAllAsync()
    {
        // Fake data for now — real DB comes later
        var stations = new List<ChargingStation>
        {
            new ChargingStation { Id = 1, Name = "Station A", Location = "New York", IsAvailable = true },
            new ChargingStation { Id = 2, Name = "Station B", Location = "Los Angeles", IsAvailable = false }
        };

        return Task.FromResult<IEnumerable<ChargingStation>>(stations);
    }
}