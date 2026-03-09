namespace EvChargingOptimizer.Application.DTOs;

public class ChargingStationResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public double PowerCapacityKw { get; set; }
    public string ConnectorType { get; set; } = string.Empty;
    public double PricePerKwh { get; set; }
}
