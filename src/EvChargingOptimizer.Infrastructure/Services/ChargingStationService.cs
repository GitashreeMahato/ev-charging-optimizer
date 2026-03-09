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

    public async Task<IEnumerable<ChargingStationResponseDto>> GetAllAsync()
    {
        return await _context.ChargingStations
            .Select(s => new ChargingStationResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                Location = s.Location,
                IsAvailable = s.IsAvailable,
                PowerCapacityKw = s.PowerCapacityKw,
                ConnectorType = s.ConnectorType,
                PricePerKwh = s.PricePerKwh
            }).ToListAsync();
    }

    public async Task<ChargingStationResponseDto?> GetByIdAsync(int id)
    {
        var station = await _context.ChargingStations.FindAsync(id);
        if (station == null) return null;

        return new ChargingStationResponseDto
        {
            Id = station.Id,
            Name = station.Name,
            Location = station.Location,
            IsAvailable = station.IsAvailable,
            PowerCapacityKw = station.PowerCapacityKw,
            ConnectorType = station.ConnectorType,
            PricePerKwh = station.PricePerKwh
        };
    }

    public async Task<ChargingStationResponseDto> CreateAsync(CreateChargingStationDto dto)
    {
        var station = new ChargingStation
        {
            Name = dto.Name,
            Location = dto.Location,
            IsAvailable = dto.IsAvailable,
            PowerCapacityKw = dto.PowerCapacityKw,
            ConnectorType = dto.ConnectorType,
            PricePerKwh = dto.PricePerKwh
        };

        _context.ChargingStations.Add(station);
        await _context.SaveChangesAsync();

        return new ChargingStationResponseDto
        {
            Id = station.Id,
            Name = station.Name,
            Location = station.Location,
            IsAvailable = station.IsAvailable,
            PowerCapacityKw = station.PowerCapacityKw,
            ConnectorType = station.ConnectorType,
            PricePerKwh = station.PricePerKwh
        };
    }

    public async Task<ChargingStationResponseDto?> UpdateAsync(int id, CreateChargingStationDto dto)
    {
        var station = await _context.ChargingStations.FindAsync(id);
        if (station == null) return null;

        station.Name = dto.Name;
        station.Location = dto.Location;
        station.IsAvailable = dto.IsAvailable;
        station.PowerCapacityKw = dto.PowerCapacityKw;
        station.ConnectorType = dto.ConnectorType;
        station.PricePerKwh = dto.PricePerKwh;

        await _context.SaveChangesAsync();

        return new ChargingStationResponseDto
        {
            Id = station.Id,
            Name = station.Name,
            Location = station.Location,
            IsAvailable = station.IsAvailable,
            PowerCapacityKw = station.PowerCapacityKw,
            ConnectorType = station.ConnectorType,
            PricePerKwh = station.PricePerKwh
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var station = await _context.ChargingStations.FindAsync(id);
        if (station == null) return false;

        _context.ChargingStations.Remove(station);
        await _context.SaveChangesAsync();
        return true;
    }
}