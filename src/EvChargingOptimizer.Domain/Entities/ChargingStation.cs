namespace EvChargingOptimizer.Domain.Entities;

public class ChargingStation
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}