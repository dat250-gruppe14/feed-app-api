using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;

namespace FeedAppApi.Mappers;

public interface IWebMapper
{
    PollWeb MapPollToWeb(Poll poll, Guid userId);
    Poll MapPollCreateRequestToInternal(PollCreateRequest request);
    UserWeb MapUserToWeb(User user);
}