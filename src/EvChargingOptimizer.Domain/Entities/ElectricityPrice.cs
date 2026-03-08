namespace EvChargingOptimizer.Domain.Entities;

public class ElectricityPrice
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double PricePerKwh { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
}