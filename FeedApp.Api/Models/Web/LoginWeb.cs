using System.ComponentModel.DataAnnotations;

namespace FeedApp.Api.Models.Web;

public class LoginWeb
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}