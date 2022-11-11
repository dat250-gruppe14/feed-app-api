using FeedApp.Common.Enums;

namespace FeedApp.Api.Models.Web;

public class UserWeb
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public UserRole Role { get; set; }
}