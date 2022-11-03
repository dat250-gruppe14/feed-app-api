using System.ComponentModel.DataAnnotations;
using FeedAppApi.Enums;

namespace FeedAppApi.Models.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Name { get; set; }

    [Required] public UserRole Role { get; set; } = UserRole.User;
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    public string PasswordSalt { get; set; }
    
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpires { get; set; }
    
    public virtual IEnumerable<Poll> Polls { get; set; }
    public virtual IEnumerable<Vote> Votes { get; set; }
}