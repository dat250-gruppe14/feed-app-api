using FeedAppApi.Mappers;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Services;
using FeedAppApi.Utils;
using Microsoft.EntityFrameworkCore;

var  webClientOrigins = "_webClientOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers();

// Services
builder.Services.AddScoped<IPollService, PollService>();
builder.Services.AddScoped<IUserService, UserService>();

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

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: webClientOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:3000",
                                              "https://localhost:3000");
                      });
});

var app = builder.Build();

app.UseCors(webClientOrigins);

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
