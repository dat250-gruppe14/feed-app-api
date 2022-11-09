using FeedAppApi.Enums;

namespace FeedAppApi.Models.Web;

public class VoteCreateRequest
{
    public UserAnswer OptionSelected { get; set; }
    public Guid PollId { get; set; }

}