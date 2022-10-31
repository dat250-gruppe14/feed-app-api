namespace FeedAppApi.Utils;

public interface IAuthUtils
{
    string GenerateToken(Guid userId);
}