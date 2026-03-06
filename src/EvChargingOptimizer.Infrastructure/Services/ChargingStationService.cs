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
}