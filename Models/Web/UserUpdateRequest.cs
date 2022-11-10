using FeedAppApi.Enums;

namespace FeedAppApi.Models.Web;


public class UserUpdateRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
}