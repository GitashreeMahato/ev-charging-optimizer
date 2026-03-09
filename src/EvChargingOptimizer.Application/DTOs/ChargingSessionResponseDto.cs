namespace EvChargingOptimizer.Application.DTOs;

public class ChargingSessionResponseDto
{
    public int Id { get; set; }
    public int ChargingStationId { get; set; }
    public int UserVehicleId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double EnergyDeliveredKwh { get; set; }
    public double TotalCostEur { get; set; }
}
