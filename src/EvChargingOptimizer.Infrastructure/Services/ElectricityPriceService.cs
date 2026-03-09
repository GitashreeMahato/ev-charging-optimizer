using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using EvChargingOptimizer.Domain.Entities;
using EvChargingOptimizer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvChargingOptimizer.Infrastructure.Services;

public class ElectricityPriceService : IElectricityPriceService
{
    private readonly AppDbContext _context;

    private static readonly TimeZoneInfo CetZone =
        TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

    public ElectricityPriceService(AppDbContext context)
    {
        _context = context;
    }

    private ElectricityPriceResponseDto MapToDto(ElectricityPrice e) => new()
    {
        Id = e.Id,
        StartTimeUtc = e.StartTime,
        EndTimeUtc = e.EndTime,
        StartTimeCet = TimeZoneInfo.ConvertTimeFromUtc(e.StartTime, CetZone)
            .ToString("yyyy-MM-dd HH:mm:ss zzz"),
        EndTimeCet = TimeZoneInfo.ConvertTimeFromUtc(e.EndTime, CetZone)
            .ToString("yyyy-MM-dd HH:mm:ss zzz"),
        PricePerKwh = e.PricePerKwh,
        Currency = e.Currency,
        Region = e.Region
    };

    public async Task<IEnumerable<ElectricityPriceResponseDto>> GetAllAsync()
    {
        var prices = await _context.ElectricityPrices.ToListAsync();
        return prices.Select(MapToDto);
    }

    public async Task<ElectricityPriceResponseDto?> GetByIdAsync(int id)
    {
        var price = await _context.ElectricityPrices.FindAsync(id);
        if (price == null) return null;
        return MapToDto(price);
    }

    public async Task<ElectricityPriceResponseDto> CreateAsync(CreateElectricityPriceDto dto)
    {
        var price = new ElectricityPrice
        {
            StartTime = dto.StartTime.ToUniversalTime(),
            EndTime = dto.EndTime.ToUniversalTime(),
            PricePerKwh = dto.PricePerKwh,
            Currency = dto.Currency,
            Region = dto.Region
        };

        _context.ElectricityPrices.Add(price);
        await _context.SaveChangesAsync();
        return MapToDto(price);
    }

    public async Task<ElectricityPriceResponseDto?> UpdateAsync(int id, CreateElectricityPriceDto dto)
    {
        var price = await _context.ElectricityPrices.FindAsync(id);
        if (price == null) return null;

        price.StartTime = dto.StartTime.ToUniversalTime();
        price.EndTime = dto.EndTime.ToUniversalTime();
        price.PricePerKwh = dto.PricePerKwh;
        price.Currency = dto.Currency;
        price.Region = dto.Region;

        await _context.SaveChangesAsync();
        return MapToDto(price);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var price = await _context.ElectricityPrices.FindAsync(id);
        if (price == null) return false;

        _context.ElectricityPrices.Remove(price);
        await _context.SaveChangesAsync();
        return true;
    }
}
