using Common.Middlewares.ExceptionMiddleware;
using ExternalServices.Integrations.Adapters.FakeStore;
using ExternalServices.Integrations.Clients.FakeStore;
using ExternalServices.Integrations.Factories;
using ExternalServices.Integrations.Interfaces.FakeStore;
using ExternalServices.Integrations.Options;
using MassTransit;
using Messaging.Common.Options;
using StockService.API.Application.Interfaces;
using StockService.API.Application.Services;
using StockService.API.Helpers;
using StockService.API.Infrastructure.Repositories;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FakeStoreOptions>(builder.Configuration.GetSection("FakeStore"));
builder.Services.AddHttpClient<IFakeStoreClient, FakeStoreClient>();
builder.Services.AddScoped<FakeStoreAdapter>();
builder.Services.AddScoped<ExternalProductProviderFactory>();
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<RomanNumeralConverter>();

var rabbitMqSettings = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>();

if (rabbitMqSettings == null)
    throw new InvalidOperationException("RabbitMQ settings could not be loaded from configuration.");

if (string.IsNullOrWhiteSpace(rabbitMqSettings.Host) ||
    string.IsNullOrWhiteSpace(rabbitMqSettings.Username) ||
    string.IsNullOrWhiteSpace(rabbitMqSettings.Password) ||
    string.IsNullOrWhiteSpace(rabbitMqSettings.VirtualHost))
{
    throw new InvalidOperationException("RabbitMQ settings (Host/Username/Password/VirtualHost) are missing or invalid.");
}

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqSettings.Host, rabbitMqSettings.VirtualHost, h =>
        {
            h.Username(rabbitMqSettings.Username);
            h.Password(rabbitMqSettings.Password);
        });
    });
});

builder.Services.AddScoped<StockManager>();

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });

    options.RejectionStatusCode = 429;
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'; script-src 'self'");
    await next();
});

app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();