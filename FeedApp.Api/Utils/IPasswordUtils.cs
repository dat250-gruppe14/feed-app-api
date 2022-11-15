namespace FeedApp.Api.Utils;

public interface IPasswordUtils
{
    string HashPassword(string password, string salt);
    bool VerifyPassword(string password, string salt, string passwordHash);
    string GenerateSalt();
}