using System.ComponentModel.DataAnnotations;
using FeedApp.Common.Enums;

namespace FeedApp.Api.Models.Web;


public class UserUpdateRequest
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    public UserRole? Role { get; set; }
}