using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;

namespace FeedAppApi.Utils;

public interface IPollUtils
{
    Records.PollStats CountPollVotes(Poll poll, Guid? userId);
    Task<IEnumerable<Poll>> GetOngoingPollsAuth(DataContext context, User? user);
    string GeneratePincode();
    //Task<IEnumerable<Poll>> GetAllPolls(DataContext context);
}