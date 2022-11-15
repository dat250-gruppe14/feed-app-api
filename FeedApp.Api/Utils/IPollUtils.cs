using FeedApp.Common.Models.Entities;
using FeedApp.Api.Proxies.Data;

namespace FeedApp.Api.Utils;

public interface IPollUtils
{
    Records.PollStats CountPollVotes(Poll poll, Guid? userId);
    Task<IEnumerable<Poll>> GetOngoingPollsAuth(DataContext context, User? user);
    string GeneratePincode();
    Task<IEnumerable<Poll>> GetAllPolls(DataContext context);
}