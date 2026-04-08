using Serilog;
using Serilog.Sinks.Elasticsearch;
using CFMS.Shared.Events;
using CFMS.CustomerService.Core.Interfaces;
using CFMS.CustomerService.Infrastructure.Data;
using CFMS.CustomerService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using MassTransit;
using CFMS.CustomerService.API.Consumers;

var builder = WebApplication.CreateBuilder(args);
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "CustomerService")
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
builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CFMS_CustomerDb")));

// DI
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// MassTransit Kafka Consumer Configuration
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CustomerLoyaltyConsumer>();

    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });

    x.AddRider(rider =>
    {
        rider.AddConsumer<CustomerLoyaltyConsumer>();

        rider.UsingKafka((context, k) =>
        {
            k.Host(builder.Configuration.GetValue<string>("Kafka:Host") ?? "localhost:9092");

            k.TopicEndpoint<OrderCreatedEvent>("order-events", "customer-group", e =>
            {
                e.ConfigureConsumer<CustomerLoyaltyConsumer>(context);
            });
        });
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
    Log.Information("Starting CustomerService...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "CustomerService terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
