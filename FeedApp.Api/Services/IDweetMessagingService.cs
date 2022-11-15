

using FeedAppApi.Mappers;
using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;


namespace FeedAppApi.Services;

public interface IDweetMessagingService
{

	public Task<HttpResponseMessage> PostPoll(Poll poll, bool CountVotes = false);
   
}