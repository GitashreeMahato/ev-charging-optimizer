namespace EvChargingOptimizer.Application.DTOs;

public class SpotPriceResponseDto
{
    public string? Updated { get; set; }
    public string? Now { get; set; }
    public string? Avg { get; set; }
    public string? Min { get; set; }
    public string? Max { get; set; }
    public List<SpotPriceDataPoint>? Data { get; set; }
}

public class SpotPriceDataPoint
{
    public string St { get; set; } = string.Empty;   // start time
    public string P { get; set; } = string.Empty;    // price
}