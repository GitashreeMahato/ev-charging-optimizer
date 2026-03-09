using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using EvChargingOptimizer.Domain.Entities;
using EvChargingOptimizer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvChargingOptimizer.Infrastructure.Services;

public class ChargingSessionService : IChargingSessionService
{
    private readonly AppDbContext _context;

    public ChargingSessionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ChargingSessionResponseDto>> GetAllAsync()
    {
        return await _context.ChargingSessions
            .Select(s => new ChargingSessionResponseDto
            {
                Id = s.Id,
                ChargingStationId = s.ChargingStationId,
                UserVehicleId = s.UserVehicleId,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                EnergyDeliveredKwh = s.EnergyDeliveredKwh,
                TotalCostEur = s.TotalCostEur
            }).ToListAsync();
    }

    public async Task<ChargingSessionResponseDto?> GetByIdAsync(int id)
    {
        var session = await _context.ChargingSessions.FindAsync(id);
        if (session == null) return null;

        return new ChargingSessionResponseDto
        {
            Id = session.Id,
            ChargingStationId = session.ChargingStationId,
            UserVehicleId = session.UserVehicleId,
            StartTime = session.StartTime,
            EndTime = session.EndTime,
            EnergyDeliveredKwh = session.EnergyDeliveredKwh,
            TotalCostEur = session.TotalCostEur
        };
    }

    public async Task<ChargingSessionResponseDto> CreateAsync(CreateChargingSessionDto dto)
    {
        var session = new ChargingSession
        {
            ChargingStationId = dto.ChargingStationId,
            UserVehicleId = dto.UserVehicleId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            EnergyDeliveredKwh = dto.EnergyDeliveredKwh,
            TotalCostEur = dto.TotalCostEur
        };

        _context.ChargingSessions.Add(session);
        await _context.SaveChangesAsync();

        return new ChargingSessionResponseDto
        {
            Id = session.Id,
            ChargingStationId = session.ChargingStationId,
            UserVehicleId = session.UserVehicleId,
            StartTime = session.StartTime,
            EndTime = session.EndTime,
            EnergyDeliveredKwh = session.EnergyDeliveredKwh,
            TotalCostEur = session.TotalCostEur
        };
    }

    public async Task<ChargingSessionResponseDto?> UpdateAsync(int id, CreateChargingSessionDto dto)
    {
        var session = await _context.ChargingSessions.FindAsync(id);
        if (session == null) return null;

        session.ChargingStationId = dto.ChargingStationId;
        session.UserVehicleId = dto.UserVehicleId;
        session.StartTime = dto.StartTime;
        session.EndTime = dto.EndTime;
        session.EnergyDeliveredKwh = dto.EnergyDeliveredKwh;
        session.TotalCostEur = dto.TotalCostEur;

        await _context.SaveChangesAsync();

        return new ChargingSessionResponseDto
        {
            Id = session.Id,
            ChargingStationId = session.ChargingStationId,
            UserVehicleId = session.UserVehicleId,
            StartTime = session.StartTime,
            EndTime = session.EndTime,
            EnergyDeliveredKwh = session.EnergyDeliveredKwh,
            TotalCostEur = session.TotalCostEur
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var session = await _context.ChargingSessions.FindAsync(id);
        if (session == null) return false;

        _context.ChargingSessions.Remove(session);
        await _context.SaveChangesAsync();
        return true;
    }
}
