using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FeedAppApi.Models.Entities;

public class Device
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string? hashedConnectionKey { get; set; }
    public string? salt { get; set; }
    public string? connectionToken { get; set; }
	public bool? isActive { get; set; }
    [Required]
    public string? Name { get; set; }
    [ForeignKey("User")]
    public Guid? UserId { get; set; }
    public virtual User? User { get; set; }
    [ForeignKey("Poll")]
    public Guid PollId { get; set; }
    public virtual Poll? ConnectedPoll { get; set; }
}
