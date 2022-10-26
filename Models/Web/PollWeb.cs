using FeedAppApi.Enums;

namespace FeedAppApi.Models.Web;

public class PollWeb
{
    public Guid Id { get; set; }
    public string Pincode { get; set; }
    public string Question { get; set; }
    public string OptionOne { get; set; }
    public string OptionTwo { get; set; }
    public PollCountsWeb Counts { get; set; }
    public UserWeb Owner { get; set; }
    public PollAccess Access { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime CreatedTime { get; set; }
    public UserAnswer? UserAnswer { get; set; }
}