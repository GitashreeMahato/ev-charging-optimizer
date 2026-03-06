using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using EvChargingOptimizer.Domain.Entities;
using EvChargingOptimizer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
namespace EvChargingOptimizer.Infrastructure.Services;

public class ChargingStationService : IChargingStationService
{

    private readonly AppDbContext _context;
    public ChargingStationService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<ChargingStation>> GetAllAsync()
    {
        return await _context.ChargingStations.ToListAsync();
    }
    public async Task<ChargingStation> CreateAsync(CreateChargingStationDto dto)
    {
        var station = new ChargingStation
        {
            Name = dto.Name,
            Location = dto.Location,
            IsAvailable = dto.isAvailable,
            PowerCapacityKw = dto.PowerCapacityKw,
            ConnectorType = dto.ConnectorType,
            PricePerKwh = dto.PricePerKwh
        };
        _context.ChargingStations.Add(station);
        await _context.SaveChangesAsync();
        return station;
    }
}