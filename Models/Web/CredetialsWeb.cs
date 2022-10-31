using System.ComponentModel.DataAnnotations;

namespace FeedAppApi.Models.Web;

public class CredetialsWeb
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}