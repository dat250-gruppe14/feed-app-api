using FeedAppApi.Proxies.Data;
using FeedAppApi.Services;
using Microsoft.EntityFrameworkCore;

var  webClientOrigins = "_webClientOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IPollService, PollService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper();
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql")));

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
