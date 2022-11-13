using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using FeedAppApi.Enums;

namespace FeedAppApi.Models.Entities;

public class DeviceVote
{
    [Key]
    public Guid Id { get; set; }
	public int Option1 { get; set; }
	public int Option2 { get; set; }
    
    [ForeignKey("VoteDevice")]
    public Guid? DeviceId { get; set; }
    public virtual Device? Device { get; set; }
    
    [ForeignKey("Poll")]
    public Guid PollId { get; set; }
    public virtual Poll? Poll { get; set; }
}
