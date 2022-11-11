using System.Net.Http;
using System.Text.Json;



using FeedAppApi.Mappers;
using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;

namespace FeedAppApi.Services;

public interface IDweetMessagingService
{

	public Task<HttpResponseMessage> Post(Poll poll);
	//public Task<HttpResponseMessage> Post(Task<IEnumerable<Poll>> polls);
}