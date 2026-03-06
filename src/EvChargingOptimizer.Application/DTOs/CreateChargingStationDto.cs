namespace EvChargingOptimizer.Application.DTOs;

public class CreateChargingStationDto
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool isAvailable { get; set; }
    public double PowerCapacityKw { get; set; }
    public string ConnectorType { get; set; } = string.Empty;
    public double PricePerKwh { get; set; }
}