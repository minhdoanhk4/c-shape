using Serilog;
using Serilog.Sinks.Elasticsearch;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;




var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "Gateway")
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

// Add YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Health Checks
builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();

// OpenTelemetry Tracing
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CFMS.Gateway"))
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://jaeger:4317");
            });
    });

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddFixedWindowLimiter("fixed-window", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100;
        opt.QueueLimit = 10;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});




var app = builder.Build();

// Enable Serilog Request Logging
app.UseSerilogRequestLogging();

app.UseRouting();

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    await next();
});

app.UseRateLimiter();


app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/auth/v1/swagger.json", "Auth API");
    options.SwaggerEndpoint("/swagger/product/v1/swagger.json", "Product API");
    options.SwaggerEndpoint("/swagger/inventory/v1/swagger.json", "Inventory API");
    options.SwaggerEndpoint("/swagger/order/v1/swagger.json", "Order API");
    options.SwaggerEndpoint("/swagger/customer/v1/swagger.json", "Customer API");
    options.SwaggerEndpoint("/swagger/shift/v1/swagger.json", "Shift API");
    options.SwaggerEndpoint("/swagger/delivery/v1/swagger.json", "Delivery API");
    options.SwaggerEndpoint("/swagger/payment/v1/swagger.json", "Payment API");
    options.SwaggerEndpoint("/swagger/promotion/v1/swagger.json", "Promotion API");
    options.SwaggerEndpoint("/swagger/reporting/v1/swagger.json", "Reporting API");
    options.SwaggerEndpoint("/swagger/franchise/v1/swagger.json", "Franchise API");
    
    options.RoutePrefix = "swagger";
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapReverseProxy();
    endpoints.MapHealthChecksUI(options => options.UIPath = "/health-ui");
});


try
{
    Log.Information("Starting API Gateway...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API Gateway terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
