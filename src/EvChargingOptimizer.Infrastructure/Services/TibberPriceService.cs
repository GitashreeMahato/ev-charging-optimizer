using System.Text;
using System.Text.Json;
using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using EvChargingOptimizer.Application.Settings;
using EvChargingOptimizer.Infrastructure.Persistence;
using EvChargingOptimizer.Domain.Entities;
using Microsoft.Extensions.Options;

namespace EvChargingOptimizer.Infrastructure.Services;

public class TibberPriceService : IExternalPriceService
{
    private readonly HttpClient _httpClient;
    private readonly TibberSettings _settings;
    private readonly AppDbContext _context;

    public TibberPriceService(
        HttpClient httpClient,
        IOptions<TibberSettings> settings,
        AppDbContext context)
    {
        _httpClient = httpClient;
        _settings = settings.Value;
        _context = context;
    }

    public async Task<IEnumerable<ElectricityPriceResponseDto>> FetchTodayPricesAsync()
    {
        var query = new
        {
            query = @"{
                viewer {
                    homes {
                        currentSubscription {
                            priceInfo {
                                today {
                                    total
                                    startsAt
                                    currency
                                    level
                                }
                                tomorrow {
                                    total
                                    startsAt
                                    currency
                                    level
                                }
                            }
                        }
                    }
                }
            }"
        };

        var request = new HttpRequestMessage(HttpMethod.Post, _settings.ApiUrl);
        request.Headers.Add("Authorization", $"Bearer {_settings.DemoToken}");
        request.Content = new StringContent(
            JsonSerializer.Serialize(query),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var tibberResponse = JsonSerializer.Deserialize<TibberPriceResponseDto>(json, options);

        var prices = tibberResponse?.Data?.Viewer?.Homes?
            .FirstOrDefault()?.CurrentSubscription?.PriceInfo?.Today
            ?? new List<TibberPrice>();

        // Save to database
        var entities = prices.Select(p => new ElectricityPrice
        {
            StartTime = p.StartsAt,
            EndTime = p.StartsAt.AddHours(1),
            PricePerKwh = p.Total,
            Currency = p.Currency,
            Region = "DE"
        }).ToList();

        _context.ElectricityPrices.AddRange(entities);
        await _context.SaveChangesAsync();

        return entities.Select(e => new ElectricityPriceResponseDto
        {
            Id = e.Id,
            StartTime = e.StartTime,
            EndTime = e.EndTime,
            PricePerKwh = e.PricePerKwh,
            Currency = e.Currency,
            Region = e.Region
        });
    }
}
