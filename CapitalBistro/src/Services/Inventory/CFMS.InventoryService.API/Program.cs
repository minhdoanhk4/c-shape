using Serilog;
using Serilog.Sinks.Elasticsearch;
using CFMS.Shared.Events;
using CFMS.InventoryService.Core.Interfaces;
using CFMS.InventoryService.Infrastructure.Data;
using CFMS.InventoryService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using MassTransit;
using CFMS.InventoryService.API.Consumers;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


var builder = WebApplication.CreateBuilder(args);
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "InventoryService")
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
builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CFMS_InventoryDb")));

// DI
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

// MassTransit Kafka Consumer Configuration
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<InventoryOrderCreatedConsumer>();

    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    x.AddRider(rider =>
    {
        rider.AddConsumer<InventoryOrderCreatedConsumer>();

        rider.UsingKafka((context, k) =>
        {
            k.Host(builder.Configuration.GetValue<string>("Kafka:Host") ?? "localhost:9092");

            k.TopicEndpoint<OrderCreatedEvent>("order-events", "inventory-group", e =>
            {
                e.ConfigureConsumer<InventoryOrderCreatedConsumer>(context);
            });
        });
    });
});

// OpenTelemetry Tracing
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CFMS.InventoryService"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation()
            .AddSource("MassTransit")
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://jaeger:4317");
            });
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
    Log.Information("Starting InventoryService...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "InventoryService terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
