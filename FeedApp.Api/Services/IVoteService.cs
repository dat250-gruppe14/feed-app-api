using FeedApp.Api.Models.Web;
using FeedApp.Common.Models.Entities;

namespace FeedApp.Api.Services;

public interface IVoteService
{
    public Task<Vote> createVote(User? user, VoteCreateRequest request);
}