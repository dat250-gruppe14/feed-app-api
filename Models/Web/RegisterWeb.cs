using System.ComponentModel.DataAnnotations;

namespace FeedAppApi.Models.Web;

public class RegisterWeb
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string Name { get; set; }
}