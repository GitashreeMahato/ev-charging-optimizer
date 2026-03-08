namespace EvChargingOptimizer.Application.DTOs;

public class CreateUserVehicleDto
{
    public string OwnerName { get; set; } = string.Empty;
    public string CarModel { get; set; } = string.Empty;
    public double BatteryCapacityKwh { get; set; }
    public string ConnectorType { get; set; } = string.Empty;
    public double CurrentBatteryPercent { get; set; }
}
