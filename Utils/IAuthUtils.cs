namespace FeedAppApi.Utils;

public interface IAuthUtils
{
    string GenerateToken(Guid userId);
    string GetUserIdFromToken(string token);
    string GenerateRefreshToken();
    bool ValidateExpiredToken(string token);
}