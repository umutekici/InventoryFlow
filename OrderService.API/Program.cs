using Common.Middlewares.ExceptionMiddleware;
using ExternalServices.Integrations.Adapters.FakeStore;
using ExternalServices.Integrations.Clients.FakeStore;
using ExternalServices.Integrations.Factories;
using ExternalServices.Integrations.Interfaces;
using ExternalServices.Integrations.Interfaces.FakeStore;
using ExternalServices.Integrations.Options;
using MassTransit;
using Messaging.Common.Options;
using OrderService.API.Application.Interfaces;
using OrderService.API.Application.Services;
using OrderService.API.Infrastructure.Repositories;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<FakeStoreOptions>(builder.Configuration.GetSection("FakeStore"));
builder.Services.AddScoped<FakeStoreAdapter>();
builder.Services.AddScoped<ExternalProductProviderFactory>();
builder.Services.AddHttpClient<IFakeStoreClient, FakeStoreClient>();
builder.Services.AddScoped<IExternalProductProvider, FakeStoreAdapter>();
builder.Services.AddScoped<OrderCreationService>();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddScoped<OrderConsumer>();

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
    x.AddConsumer<OrderConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("order-queue", e =>
        {
            e.ConfigureConsumer<OrderConsumer>(context);
        });
    });
});

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
