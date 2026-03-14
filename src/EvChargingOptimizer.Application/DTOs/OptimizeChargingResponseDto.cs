namespace EvChargingOptimizer.Application.DTOs;

public class OptimizeChargingResponseDto
{
    public DateTime RecommendedStartTimeUtc { get; set; }
    public DateTime RecommendedEndTimeUtc { get; set; }
    public string RecommendedStartTimeCet { get; set; } = string.Empty;
    public string RecommendedEndTimeCet { get; set; } = string.Empty;
    public double EnergyNeededKwh { get; set; }
    public double ChargingDurationHours { get; set; }
    public double EstimatedCostEur { get; set; }
    public double AveragePricePerKwh { get; set; }
    public string CheapestWindow { get; set; } = string.Empty;
    // ADD these three fields:
    public int VehicleId { get; set; }
    public int StationId { get; set; }
    public int SessionId { get; set; }
}