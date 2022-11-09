namespace FeedAppApi.Models.Web;

public class UserWithTokenWeb
{
    public UserWeb User { get; set; }
    public string Token { get; set; }
}