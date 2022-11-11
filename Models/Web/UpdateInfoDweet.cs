

namespace FeedAppApi.Models.Web;

public class UpdateInfoDweet
{
    public List<string> NewlyUpdatedPolls { get; set; }
    public DateTime UpdateTime { get; set; }
    public string FirstPoll { get; set; }
}