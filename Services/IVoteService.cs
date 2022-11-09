using FeedAppApi.Models.Web;
using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Enums;


namespace FeedAppApi.Services;


public interface IVoteService
{
    public Task<Vote?> createVote(Guid userId, VoteCreateRequest request);


}