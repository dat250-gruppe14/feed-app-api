using System.Net.Http;
using System.Text.Json;
using FeedAppApi.Proxies.Data;



using FeedAppApi.Mappers;
using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;
using System.Threading.Tasks;

namespace FeedAppApi.Services;

public interface IDweetMessagingService
{

    public Guid? GetNextGuid(Guid id);
    public Task<HttpResponseMessage> init(DataContext context);
	public Task<HttpResponseMessage> PostUpdate(Poll poll, DataContext context);
    public Task<HttpResponseMessage> PostNew(Poll poll, DataContext context);
    public Task<HttpResponseMessage> PostPollDweet(string dweeturl, PollMessageDweet md);
    public Task<HttpResponseMessage> PostUpdateInfoDweet(UpdateInfoDweet updated);
}