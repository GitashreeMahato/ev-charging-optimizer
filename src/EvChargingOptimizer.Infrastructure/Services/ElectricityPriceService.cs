using EvChargingOptimizer.Application.DTOs;
using EvChargingOptimizer.Application.Interfaces;
using EvChargingOptimizer.Domain.Entities;
using EvChargingOptimizer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvChargingOptimizer.Infrastructure.Services;

public class ElectricityPriceService : IElectricityPriceService
{
    private readonly AppDbContext _context;

    public ElectricityPriceService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ElectricityPriceResponseDto>> GetAllAsync()
    {
        return await _context.ElectricityPrices
            .Select(p => new ElectricityPriceResponseDto
            {
                Id = p.Id,
                StartTime = p.StartTime,
                EndTime = p.EndTime,
                PricePerKwh = p.PricePerKwh,
                Currency = p.Currency,
                Region = p.Region
            }).ToListAsync();
    }

    public async Task<ElectricityPriceResponseDto?> GetByIdAsync(int id)
    {
        var price = await _context.ElectricityPrices.FindAsync(id);
        if (price == null) return null;

        return new ElectricityPriceResponseDto
        {
            Id = price.Id,
            StartTime = price.StartTime,
            EndTime = price.EndTime,
            PricePerKwh = price.PricePerKwh,
            Currency = price.Currency,
            Region = price.Region
        };
    }

    public async Task<ElectricityPriceResponseDto> CreateAsync(CreateElectricityPriceDto dto)
    {
        var price = new ElectricityPrice
        {
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            PricePerKwh = dto.PricePerKwh,
            Currency = dto.Currency,
            Region = dto.Region
        };

        _context.ElectricityPrices.Add(price);
        await _context.SaveChangesAsync();

        return new ElectricityPriceResponseDto
        {
            Id = price.Id,
            StartTime = price.StartTime,
            EndTime = price.EndTime,
            PricePerKwh = price.PricePerKwh,
            Currency = price.Currency,
            Region = price.Region
        };
    }

    public async Task<ElectricityPriceResponseDto?> UpdateAsync(int id, CreateElectricityPriceDto dto)
    {
        var price = await _context.ElectricityPrices.FindAsync(id);
        if (price == null) return null;

        price.StartTime = dto.StartTime;
        price.EndTime = dto.EndTime;
        price.PricePerKwh = dto.PricePerKwh;
        price.Currency = dto.Currency;
        price.Region = dto.Region;

        await _context.SaveChangesAsync();

        return new ElectricityPriceResponseDto
        {
            Id = price.Id,
            StartTime = price.StartTime,
            EndTime = price.EndTime,
            PricePerKwh = price.PricePerKwh,
            Currency = price.Currency,
            Region = price.Region
        };
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
