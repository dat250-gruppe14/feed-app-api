using FeedApp.Common.Models.Entities;


namespace FeedApp.Api.Services;

public interface IDweetMessagingService
{

	public Task<HttpResponseMessage> PostPoll(Poll poll, bool CountVotes = false);
   
}