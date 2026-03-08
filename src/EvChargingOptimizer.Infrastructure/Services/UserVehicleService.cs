using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using EvChargingOptimizer.Domain.Entities;
using EvChargingOptimizer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvChargingOptimizer.Infrastructure.Services;

public class UserVehicleService : IUserVehicleService
{
    private readonly AppDbContext _context;

    public UserVehicleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserVehicleResponseDto>> GetAllAsync()
    {
        return await _context.UserVehicles
            .Select(v => new UserVehicleResponseDto
            {
                Id = v.Id,
                OwnerName = v.OwnerName,
                CarModel = v.CarModel,
                BatteryCapacityKwh = v.BatteryCapacityKwh,
                ConnectorType = v.ConnectorType,
                CurrentBatteryPercent = v.CurrentBatteryPercent
            }).ToListAsync();
    }

    public async Task<UserVehicleResponseDto?> GetByIdAsync(int id)
    {
        var vehicle = await _context.UserVehicles.FindAsync(id);
        if (vehicle == null) return null;

        return new UserVehicleResponseDto
        {
            Id = vehicle.Id,
            OwnerName = vehicle.OwnerName,
            CarModel = vehicle.CarModel,
            BatteryCapacityKwh = vehicle.BatteryCapacityKwh,
            ConnectorType = vehicle.ConnectorType,
            CurrentBatteryPercent = vehicle.CurrentBatteryPercent
        };
    }

    public async Task<UserVehicleResponseDto> CreateAsync(CreateUserVehicleDto dto)
    {
        var vehicle = new UserVehicle
        {
            OwnerName = dto.OwnerName,
            CarModel = dto.CarModel,
            BatteryCapacityKwh = dto.BatteryCapacityKwh,
            ConnectorType = dto.ConnectorType,
            CurrentBatteryPercent = dto.CurrentBatteryPercent
        };

        _context.UserVehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        return new UserVehicleResponseDto
        {
            Id = vehicle.Id,
            OwnerName = vehicle.OwnerName,
            CarModel = vehicle.CarModel,
            BatteryCapacityKwh = vehicle.BatteryCapacityKwh,
            ConnectorType = vehicle.ConnectorType,
            CurrentBatteryPercent = vehicle.CurrentBatteryPercent
        };
    }

    public async Task<UserVehicleResponseDto?> UpdateAsync(int id, CreateUserVehicleDto dto)
    {
        var vehicle = await _context.UserVehicles.FindAsync(id);
        if (vehicle == null) return null;

        vehicle.OwnerName = dto.OwnerName;
        vehicle.CarModel = dto.CarModel;
        vehicle.BatteryCapacityKwh = dto.BatteryCapacityKwh;
        vehicle.ConnectorType = dto.ConnectorType;
        vehicle.CurrentBatteryPercent = dto.CurrentBatteryPercent;

        await _context.SaveChangesAsync();

        return new UserVehicleResponseDto
        {
            Id = vehicle.Id,
            OwnerName = vehicle.OwnerName,
            CarModel = vehicle.CarModel,
            BatteryCapacityKwh = vehicle.BatteryCapacityKwh,
            ConnectorType = vehicle.ConnectorType,
            CurrentBatteryPercent = vehicle.CurrentBatteryPercent
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vehicle = await _context.UserVehicles.FindAsync(id);
        if (vehicle == null) return false;

        _context.UserVehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
        return true;
    }
}
