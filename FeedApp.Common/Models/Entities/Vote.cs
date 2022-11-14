using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FeedApp.Common.Enums;

namespace FeedApp.Common.Models.Entities;

public class Vote
{
    [Key]
    public Guid Id { get; set; }
    public UserAnswer OptionSelected { get; set; }
    
    [ForeignKey("User")]
    public Guid? UserId { get; set; }
    public virtual User? User { get; set; }
    
    [ForeignKey("Poll")]
    public Guid PollId { get; set; }
    public virtual Poll Poll { get; set; }
}