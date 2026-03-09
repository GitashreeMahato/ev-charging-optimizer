namespace EvChargingOptimizer.Application.DTOs;

public class ElectricityPriceResponseDto
{
    public int Id { get; set; }
    public DateTime StartTimeUtc { get; set; }
    public DateTime EndTimeUtc { get; set; }
    public string StartTimeCet { get; set; } = string.Empty;  // ← CET for display
    public string EndTimeCet { get; set; } = string.Empty;   // ← CET for display
    public double PricePerKwh { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
}
