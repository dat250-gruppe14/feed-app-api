using FeedApp.Common.Models.Entities;

namespace FeedApp.Api.Utils;

public interface IAuthUtils
{
    string GenerateToken(Guid userId);
    string? GetUserIdFromToken(string token);
    string GenerateRefreshToken();
    bool ValidateExpiredToken(string token);
    string? GetTokenFromHttpContext(HttpContext httpContext);
    User? GetLoggedInUserFromHttpContext(HttpContext httpContext);
}