using FeedApp.Service;
using FeedApp.Messaging.Sender;
using FeedApp.Messaging.Options;
using FeedApp.Api.Proxies.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddScoped<PublishService>();
builder.Services.AddSingleton<PeriodicHostedService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<PeriodicHostedService>());

// Messaging
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddTransient<IPollExpiredSender, PollExpiredSender>();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));
    options.UseLazyLoadingProxies();
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/background", (
    PeriodicHostedService service) =>
{
    return new PeriodicHostedServiceState(service.IsEnabled);
});

app.MapMethods("/background", new[] { "PATCH" }, (
    PeriodicHostedServiceState state,
    PeriodicHostedService service) =>
{
    service.IsEnabled = state.IsEnabled;
});

app.Run();
