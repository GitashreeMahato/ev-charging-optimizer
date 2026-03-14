using EvChargingOptimizer.Application.Interfaces;
using EvChargingOptimizer.Application.Settings;
using EvChargingOptimizer.Infrastructure.Services;
using EvChargingOptimizer.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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
builder.Services.AddHostedService<PriceUpdateBackgroundService>();
// Register AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Removed UseHttpsRedirection to avoid warning
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Auto-fetch prices on startup
using (var scope = app.Services.CreateScope())
{
    var priceService = scope.ServiceProvider.GetRequiredService<IExternalPriceService>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Auto-fetching electricity prices on startup...");
        var prices = await priceService.FetchTodayPricesAsync();
        logger.LogInformation("Startup price fetch completed. Fetched {Count} prices.", prices.Count());
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Startup price fetch failed. App will continue without latest prices.");
    }
}

app.Run();













