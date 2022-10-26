using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FeedAppApi.Enums;

namespace FeedAppApi.Models.Entities;

public class Poll {
	
	[Key]
	public Guid Id { get; set; }
	[Required]
	public string? Pincode { get; set; }
	[Required]
	public string? Question { get; set; }
	[Required]
	public string? OptionOne { get; set; }
	[Required]
	public string? OptionTwo { get; set; }
	[Required]
	public PollAccess? Access { get; set; }
	[Required]
	public DateTime? StartTime { get; set; }
	[Required]
	public DateTime? EndTime { get; set; }
	[Required] public DateTime CreatedTime { get; set; }

	[ForeignKey("Owner")]
	public Guid OwnerId { get; set; }
	public virtual User Owner { get; set; }
	public virtual IEnumerable<Vote> Votes { get; set; }
}
