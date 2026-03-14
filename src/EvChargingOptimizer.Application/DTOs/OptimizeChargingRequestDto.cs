namespace EvChargingOptimizer.Application.DTOs;

public class OptimizeChargingRequestDto
{
    public double CurrentBatteryPercent { get; set; }
    public double TargetBatteryPercent { get; set; }
    public double BatteryCapacityKwh { get; set; }
    public double ChargerPowerKw { get; set; }
    public DateTime DeadLine { get; set; }
    // ADD these two fields:
    public int VehicleId { get; set; }
    public int StationId { get; set; }
}