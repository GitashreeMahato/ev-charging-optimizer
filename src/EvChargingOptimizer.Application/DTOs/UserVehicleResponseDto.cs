namespace EvChargingOptimizer.Application.DTOs;

public class UserVehicleResponseDto
{
    public int Id { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string CarModel { get; set; } = string.Empty;
    public double BatteryCapacityKwh { get; set; }
    public string ConnectorType { get; set; } = string.Empty;
    public double CurrentBatteryPercent { get; set; }
}
