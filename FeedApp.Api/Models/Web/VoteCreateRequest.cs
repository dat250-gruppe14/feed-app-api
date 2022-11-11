using FeedApp.Common.Enums;

namespace FeedAppApi.Models.Web;

public class VoteCreateRequest
{
    public UserAnswer OptionSelected { get; set; }
    public String PollPincode { get; set; }

}