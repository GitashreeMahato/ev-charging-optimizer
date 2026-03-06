using EvChargingOptimizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvChargingOptimizer.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ChargingStation> ChargingStations => Set<ChargingStation>();
}