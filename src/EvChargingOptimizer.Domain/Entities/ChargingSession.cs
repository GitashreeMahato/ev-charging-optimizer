namespace EvChargingOptimizer.Domain.Entities;

public class ChargingSession
{
    public int Id { get; set; }
    public int ChargingStationId { get; set; }
    public int UserVehicleId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double EnergyDeliveredKwh { get; set; }
    public double TotalCostEur { get; set; }

    // Navigation properties
    public ChargingStation ChargingStation { get; set; } = null!;
    public UserVehicle UserVehicle { get; set; } = null!;
}


