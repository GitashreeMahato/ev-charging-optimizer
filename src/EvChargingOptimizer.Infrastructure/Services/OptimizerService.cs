using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using EvChargingOptimizer.Domain.Entities;
using EvChargingOptimizer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvChargingOptimizer.Infrastructure.Services;

public class OptimizerService : IOptimizerService
{
    private readonly AppDbContext _context;
    private static readonly TimeZoneInfo CetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

    public OptimizerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OptimizeChargingResponseDto> OptimizeAsync(OptimizeChargingRequestDto request)
    {
        // Step 1 — Validate Vehicle exists
        var vehicle = await _context.UserVehicles.FindAsync(request.VehicleId);
        if (vehicle == null)
            throw new InvalidOperationException($"Vehicle with ID {request.VehicleId} not found.");

        // Step 2 — Validate Station exists
        var station = await _context.ChargingStations.FindAsync(request.StationId);
        if (station == null)
            throw new InvalidOperationException($"Charging station with ID {request.StationId} not found.");

        // Step 3 — Calculate energy needed
        var energyNeededKwh = (request.TargetBatteryPercent - request.CurrentBatteryPercent) / 100.0 * request.BatteryCapacityKwh;

        // Step 4 — Charging duration in hours
        var chargingDurationHours = energyNeededKwh / request.ChargerPowerKw;

        // Step 5 — Calculate number of 15 min slots needed
        var slotsNeeded = (int)Math.Ceiling(chargingDurationHours * 4);

        double estimatedCost;
        string cheapestWindow;
        double avgPrice;
        DateTime startUtc;
        DateTime endUtc;

        // Step 6 — Branch: Public charger (fixed price) vs Home charger (spot price)
        if (station.PricePerKwh > 0)
        {
            // ⚡ PUBLIC CHARGER — fixed price, no need to optimize by time
            // Just charge as soon as possible (now → now + duration)
            estimatedCost = Math.Round(energyNeededKwh * station.PricePerKwh, 4);
            avgPrice = station.PricePerKwh;

            startUtc = DateTime.UtcNow;
            endUtc = startUtc.AddHours(chargingDurationHours);

            var startCetPublic = TimeZoneInfo.ConvertTimeFromUtc(startUtc, CetZone);
            var endCetPublic = TimeZoneInfo.ConvertTimeFromUtc(endUtc, CetZone);
            cheapestWindow = $"{startCetPublic:HH:mm} - {endCetPublic:HH:mm} CET (fixed price)";
        }
        else
        {
            // 🏠 HOME CHARGER — use spot prices, find cheapest window
            var deadlineUtc = request.DeadLine.ToUniversalTime();
            var nowUtc = DateTime.UtcNow;

            var prices = await _context.ElectricityPrices
                .Where(p => p.StartTime > nowUtc && p.StartTime < deadlineUtc)
                .OrderBy(p => p.StartTime)
                .AsNoTracking()
                .ToListAsync();

            if (prices.Count < slotsNeeded)
                throw new InvalidOperationException(
                    $"Not enough price data available. Need {slotsNeeded} slots but only {prices.Count} available. Please fetch today's prices first.");

            // Sliding window — find cheapest consecutive slots
            double cheapestCost = double.MaxValue;
            int bestStartIndex = 0;

            for (int i = 0; i <= prices.Count - slotsNeeded; i++)
            {
                var windowPrices = prices.Skip(i).Take(slotsNeeded).ToList();
                var windowCost = windowPrices.Sum(p => p.PricePerKwh * 0.25 * request.ChargerPowerKw);

                if (windowCost < cheapestCost)
                {
                    cheapestCost = windowCost;
                    bestStartIndex = i;
                }
            }

            var bestWindow = prices.Skip(bestStartIndex).Take(slotsNeeded).ToList();
            startUtc = bestWindow.First().StartTime;
            endUtc = bestWindow.Last().EndTime;
            avgPrice = bestWindow.Average(p => p.PricePerKwh);
            estimatedCost = Math.Round(cheapestCost, 4);

            var startCet = TimeZoneInfo.ConvertTimeFromUtc(startUtc, CetZone);
            var endCet = TimeZoneInfo.ConvertTimeFromUtc(endUtc, CetZone);
            cheapestWindow = $"{startCet:HH:mm} - {endCet:HH:mm} CET";
        }

        // Step 7 — Auto-save ChargingSession
        var session = new ChargingSession
        {
            UserVehicleId = request.VehicleId,
            ChargingStationId = request.StationId,
            StartTime = startUtc,
            EndTime = endUtc,
            EnergyDeliveredKwh = Math.Round(energyNeededKwh, 2),
            TotalCostEur = estimatedCost
        };

        _context.ChargingSessions.Add(session);
        await _context.SaveChangesAsync();

        // Step 8 — Convert to CET for display
        var startCetFinal = TimeZoneInfo.ConvertTimeFromUtc(startUtc, CetZone);
        var endCetFinal = TimeZoneInfo.ConvertTimeFromUtc(endUtc, CetZone);

        return new OptimizeChargingResponseDto
        {
            RecommendedStartTimeUtc = startUtc,
            RecommendedEndTimeUtc = endUtc,
            RecommendedStartTimeCet = startCetFinal.ToString("yyyy-MM-dd HH:mm:ss"),
            RecommendedEndTimeCet = endCetFinal.ToString("yyyy-MM-dd HH:mm:ss"),
            EnergyNeededKwh = Math.Round(energyNeededKwh, 2),
            ChargingDurationHours = Math.Round(chargingDurationHours, 2),
            EstimatedCostEur = estimatedCost,
            AveragePricePerKwh = Math.Round(avgPrice, 4),
            CheapestWindow = cheapestWindow,
            VehicleId = request.VehicleId,
            StationId = request.StationId,
            SessionId = session.Id
        };
    }
}