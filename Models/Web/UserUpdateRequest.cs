using System.ComponentModel.DataAnnotations;
using FeedAppApi.Enums;

namespace FeedAppApi.Models.Web;


public class UserUpdateRequest
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    public UserRole? Role { get; set; }
}