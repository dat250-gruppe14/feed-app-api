using System.ComponentModel.DataAnnotations;
using FeedAppApi.Enums;

namespace FeedAppApi.Models.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string? Email { get; set; } // Trenger vi mail?
    [Required]
    public string? Name { get; set; }
    [Required]
    public UserRole? Role { get; set; }
    
    public virtual IEnumerable<Poll> Polls { get; set; }
    public virtual IEnumerable<Vote> Votes { get; set; }
}