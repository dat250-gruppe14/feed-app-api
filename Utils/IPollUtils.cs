using FeedAppApi.Models.Entities;

namespace FeedAppApi.Utils;

public interface IPollUtils
{
    Records.PollStats CountPollVotes(Poll poll, Guid userId);
}