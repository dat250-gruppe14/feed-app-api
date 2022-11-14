using FeedApp.Common.Enums;

namespace FeedApp.Api.Models.Web;

public class VoteCreateRequest
{
    public UserAnswer OptionSelected { get; set; }
    public string? PollPincode { get; set; }

}