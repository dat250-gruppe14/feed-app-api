using FeedAppApi.Models.Web;
using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Enums;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedAppApi.Services;


public interface IVoteService
{
    public Task<Vote?> createVote(User user, VoteCreateRequest request);
}
