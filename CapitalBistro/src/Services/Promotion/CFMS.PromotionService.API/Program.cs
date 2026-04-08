using Serilog;
using Serilog.Sinks.Elasticsearch;
using CFMS.PromotionService.Core.Interfaces;
using CFMS.PromotionService.Infrastructure.Data;
using CFMS.PromotionService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "PromotionService")
    .Enrich.WithEnvironmentName()
    .Enrich.WithProcessId()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration.GetValue<string>("Serilog:ElasticsearchUrl") ?? "http://elasticsearch:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "cfms-logs-{0:yyyy.MM.dd}"
    })
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();


// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core
builder.Services.AddDbContext<PromotionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CFMS_PromotionDb")));

// DI
builder.Services.AddScoped<IPromotionRepository, PromotionRepository>();

// Redis Cache Configuration
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("Redis:ConnectionString") ?? "redis:6379";
    options.InstanceName = "CFMS_Promotion_";
});

// JWT Authentication
var secretKey = builder.Configuration["JwtSettings:SecretKey"];
if (string.IsNullOrEmpty(secretKey))
{
    throw new ArgumentNullException("JwtSettings:SecretKey is not configured.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" 
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable Serilog Request Logging
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting PromotionService...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "PromotionService terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
