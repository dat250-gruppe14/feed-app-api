using System.Text;
using FeedAppApi.Mappers;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Services;
using FeedAppApi.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var  webClientOrigins = "_webClientOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Authentication (https://www.infoworld.com/article/3669188/how-to-implement-jwt-authentication-in-aspnet-core-6.html)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        LifetimeValidator = (_, expires, _, _) => expires >= DateTime.UtcNow,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers()
    .AddNewtonsoftJson();

// Services
builder.Services.AddScoped<IPollService, PollService>();
builder.Services.AddScoped<IAuthUtils, AuthUtils>();
builder.Services.AddScoped<IUserService, UserService>();

// Mappers
builder.Services.AddScoped<IWebMapper, WebMapper>();

// Utils
builder.Services.AddScoped<IPollUtils, PollUtils>();
builder.Services.AddScoped<IAuthUtils, AuthUtils>();
builder.Services.AddScoped<IPasswordUtils, PasswordUtils>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
});

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
app.UseAuthentication();
app.UseAuthorization();

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
