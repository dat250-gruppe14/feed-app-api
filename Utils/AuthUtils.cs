using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FeedAppApi.Utils;

public class AuthUtils : IAuthUtils
{
    public string GenerateToken(Guid userId)
    {
        var issuer = "https://feedapp.com/";
        var audience = "https://feedapp.com/";
        var key = Encoding.ASCII.GetBytes
            ("This is a sample secret key - please don't use in production environment.'");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("userId", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,
                    userId.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
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
}