using System.Data.Common;

namespace EvChargingOptimizer.Domain.Entities;

public class UserVehicle
{

    public int Id { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string CarModel { get; set; } = string.Empty;
    public double BatteryCapacityKwh { get; set; }
    public string ConnectorType { get; set; } = string.Empty;
    public double CurrentBatteryPercent { get; set; }
}