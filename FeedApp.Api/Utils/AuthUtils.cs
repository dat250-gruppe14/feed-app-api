using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FeedApp.Common.Models.Entities;
using Microsoft.IdentityModel.Tokens;

namespace FeedApp.Api.Utils;

public class AuthUtils : IAuthUtils
{
    private readonly IConfiguration _configuration;

    public AuthUtils(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GenerateToken(Guid userId)
    {
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var key = Encoding.ASCII.GetBytes
            (_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,
                    userId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return tokenHandler.WriteToken(token);
    }

    public string? GetUserIdFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadJwtToken(token);
            var subClaim = decodedToken.Claims.First(p => p.Type == "sub");
            return subClaim.Value;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public bool ValidateExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken validatedToken;
        try
        {
            var result = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false
            }, out validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public string? GetTokenFromHttpContext(HttpContext httpContext)
    {
        string? token;
        try
        {
            return httpContext.Request.Headers.Authorization[0][7..];
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }

    public User? GetLoggedInUserFromHttpContext(HttpContext httpContext)
    {
        return (User) httpContext.Items["user"]!;
    }
}
