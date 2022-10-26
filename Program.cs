using FeedAppApi.Mappers;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Services;
using FeedAppApi.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

// Services
builder.Services.AddScoped<IPollService, PollService>();

// Mappers
builder.Services.AddScoped<IWebMapper, WebMapper>();

// Utils
builder.Services.AddScoped<IPollUtils, PollUtils>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));
    options.UseLazyLoadingProxies();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
