using EvChargingOptimizer.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EvChargingOptimizer.Infrastructure.Services;

public class PriceUpdateBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PriceUpdateBackgroundService> _logger;

    // Run every day at 14:00 CET (when German day-ahead prices are published)
    private readonly TimeSpan _scheduledTime = new TimeSpan(14, 0, 0);

    private static readonly TimeZoneInfo CetZone =
        TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

    public PriceUpdateBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<PriceUpdateBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Price Update Background Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = GetDelayUntilNextRun();
            _logger.LogInformation("Next price fetch scheduled in {Minutes} minutes at 14:00 CET.",
                (int)delay.TotalMinutes);

            await Task.Delay(delay, stoppingToken);

            if (!stoppingToken.IsCancellationRequested)
            {
                await FetchPricesAsync();
            }
        }
    }

    private TimeSpan GetDelayUntilNextRun()
    {
        var nowCet = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, CetZone);
        var nextRun = nowCet.Date.Add(_scheduledTime);

        // If 14:00 already passed today, schedule for tomorrow
        if (nowCet >= nextRun)
        {
            nextRun = nextRun.AddDays(1);
        }

        return nextRun - nowCet;
    }

    private async Task FetchPricesAsync()
    {
        _logger.LogInformation("Starting scheduled price fetch at {Time}", DateTime.UtcNow);

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var priceService = scope.ServiceProvider.GetRequiredService<IExternalPriceService>();
            var prices = await priceService.FetchTodayPricesAsync();
            _logger.LogInformation("Scheduled price fetch completed. Fetched {Count} prices.",
                prices.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Scheduled price fetch failed.");
        }
    }
}