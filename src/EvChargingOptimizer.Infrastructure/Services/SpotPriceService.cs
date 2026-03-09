using System.Text.Json;
using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using EvChargingOptimizer.Application.Settings;
using EvChargingOptimizer.Infrastructure.Persistence;
using EvChargingOptimizer.Domain.Entities;
using Microsoft.Extensions.Options;

namespace EvChargingOptimizer.Infrastructure.Services;

public class SpotPriceService : IExternalPriceService
{
    private readonly HttpClient _httpClient;
    private readonly SpotPriceSettings _settings;
    private readonly AppDbContext _context;

    // German timezone
    private static readonly TimeZoneInfo CetZone =
        TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

    public SpotPriceService(
        HttpClient httpClient,
        IOptions<SpotPriceSettings> settings,
        AppDbContext context)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _context = context;
    }

    public async Task<IEnumerable<ElectricityPriceResponseDto>> FetchTodayPricesAsync()
    {
        var response = await _httpClient.GetAsync(_settings.ApiUrl);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        Console.WriteLine("SpotPrice raw response: " + json[..200]);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var spotResponse = JsonSerializer.Deserialize<SpotPriceResponseDto>(json, options);
        Console.WriteLine("Data points count: " + (spotResponse?.Data?.Count ?? 0));

        var today = DateTime.UtcNow.Date;

        // Filter only today's prices
        var todayPrices = spotResponse?.Data?
            .Where(d => DateTime.Parse(d.St).ToUniversalTime().Date == today)
            .ToList() ?? new List<SpotPriceDataPoint>();

        Console.WriteLine("Today's prices count: " + todayPrices.Count);

        var entities = todayPrices.Select(d =>
        {
            // Parse and store as UTC
            var utcStart = DateTime.Parse(d.St).ToUniversalTime();
            var utcEnd = utcStart.AddMinutes(15);

            return new ElectricityPrice
            {
                StartTime = utcStart,           // ← store UTC
                EndTime = utcEnd,               // ← store UTC
                PricePerKwh = double.Parse(d.P, System.Globalization.CultureInfo.InvariantCulture),
                Currency = "EUR",
                Region = "DE"
            };
        }).ToList();

        _context.ElectricityPrices.AddRange(entities);
        await _context.SaveChangesAsync();

        return entities.Select(e => new ElectricityPriceResponseDto
        {
            Id = e.Id,
            StartTimeUtc = e.StartTime,
            EndTimeUtc = e.EndTime,
            // Convert UTC to CET for display
            StartTimeCet = TimeZoneInfo.ConvertTimeFromUtc(e.StartTime, CetZone)
                .ToString("yyyy-MM-dd HH:mm:ss zzz"),
            EndTimeCet = TimeZoneInfo.ConvertTimeFromUtc(e.EndTime, CetZone)
                .ToString("yyyy-MM-dd HH:mm:ss zzz"),
            PricePerKwh = e.PricePerKwh,
            Currency = e.Currency,
            Region = e.Region
        });
    }
}