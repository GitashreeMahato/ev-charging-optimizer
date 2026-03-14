using EvChargingOptimizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvChargingOptimizer.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ChargingStation> ChargingStations => Set<ChargingStation>();
    public DbSet<UserVehicle> UserVehicles => Set<UserVehicle>();
    public DbSet<ChargingSession> ChargingSessions => Set<ChargingSession>();
    public DbSet<ElectricityPrice> ElectricityPrices => Set<ElectricityPrice>();
    public DbSet<User> Users => Set<User>();
}