using FeedApp.Common.Enums;

namespace FeedApp.Api.Models.Web;

public class PollCreateRequest
{
	public string Question { get; set; }
	public string OptionOne { get; set; }
	public string OptionTwo { get; set; }
	public PollAccess Access { get; set; }
	public DateTime? StartTime { get; set; }
	public DateTime? EndTime { get; set; }
	public Guid OwnerId { get; set; }
}
