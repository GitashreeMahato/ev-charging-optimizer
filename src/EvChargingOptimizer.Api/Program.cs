using EvChargingOptimizer.Application.Interfaces;
using EvChargingOptimizer.Application.Settings;
using EvChargingOptimizer.Infrastructure.Services;
using EvChargingOptimizer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  Register AppDbContext

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Tibber settings
builder.Services.Configure<TibberSettings>(
    builder.Configuration.GetSection("Tibber"));

// HttpClient for Tibber
builder.Services.AddHttpClient<IExternalPriceService, TibberPriceService>();
// Register our service
builder.Services.AddScoped<IChargingStationService, ChargingStationService>();
builder.Services.AddScoped<IUserVehicleService, UserVehicleService>();
builder.Services.AddScoped<IChargingSessionService, ChargingSessionService>();
builder.Services.AddScoped<IElectricityPriceService, ElectricityPriceService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Removed UseHttpsRedirection to avoid warning
app.UseAuthorization();
app.MapControllers();
app.Run();













