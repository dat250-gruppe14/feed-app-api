using FeedAppApi.Models.Web;
using FeedApp.Api.Proxies.Data;
using FeedApp.Common.Models.Entities;
using FeedApp.Common.Enums;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedApp.Api.Services;


public interface IVoteService
{
    public Task<Vote?> createVote(User user, VoteCreateRequest request);



}