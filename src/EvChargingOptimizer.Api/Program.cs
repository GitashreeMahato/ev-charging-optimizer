using EvChargingOptimizer.Application.Interfaces;
using EvChargingOptimizer.Application.Settings;
using EvChargingOptimizer.Infrastructure.Services;
using EvChargingOptimizer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

// convert UTC to local time
// AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//  Register AppDbContext

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// SpotPrice settings
builder.Services.Configure<SpotPriceSettings>(
    builder.Configuration.GetSection("SpotPrice"));

// HttpClient for SpotPrice
builder.Services.AddHttpClient<IExternalPriceService, SpotPriceService>();

// Register our service
builder.Services.AddScoped<IChargingStationService, ChargingStationService>();
builder.Services.AddScoped<IUserVehicleService, UserVehicleService>();
builder.Services.AddScoped<IChargingSessionService, ChargingSessionService>();
builder.Services.AddScoped<IElectricityPriceService, ElectricityPriceService>();
builder.Services.AddScoped<IOptimizerService, OptimizerService>();
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













