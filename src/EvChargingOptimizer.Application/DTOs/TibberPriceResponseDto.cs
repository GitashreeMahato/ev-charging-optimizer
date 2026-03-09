namespace EvChargingOptimizer.Application.DTOs;

public class TibberPriceResponseDto
{
    public TibberData? Data { get; set; }
}

public class TibberData
{
    public TibberViewer? Viewer { get; set; }
}

public class TibberViewer
{
    public List<TibberHome>? Homes { get; set; }
}

public class TibberHome
{
    public TibberSubscription? CurrentSubscription { get; set; }
}

public class TibberSubscription
{
    public TibberPriceInfo? PriceInfo { get; set; }
}

public class TibberPriceInfo
{
    public List<TibberPrice>? Today { get; set; }
    public List<TibberPrice>? Tomorrow { get; set; }
}

public class TibberPrice
{
    public double Total { get; set; }
    public DateTime StartsAt { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
}
