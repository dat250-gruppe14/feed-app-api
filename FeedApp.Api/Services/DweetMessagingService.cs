using FeedApp.Api.Mappers;
using FeedApp.Common.Models.Entities;
using FeedApp.Api.Models.Web;



namespace FeedApp.Api.Services;

public class DweetMessagingService : IDweetMessagingService
{

    private string _basePostUrl = "https://dweet.io/dweet/for/";
    private string _dweetThing = "FeedApp";
    private string _baseGetUrl = "https://dweet.io/get/latest/dweet/for/";

    private HttpClient _client;

    private readonly IWebMapper _webMapper;

    public DweetMessagingService(IWebMapper webMapper)
    {
        _webMapper = webMapper;
        _client = new()
        {
            BaseAddress = new Uri(_basePostUrl),
        };
        
    }


    public async Task<HttpResponseMessage> PostPoll(Poll poll, bool CountVotes = false)
    {
        PollWeb pw = _webMapper.MapPollToWeb(poll, null, CountVotes);
        string PollEndpoint = _dweetThing + pw.Id.ToString();
        //Post poll
        await _client.PostAsJsonAsync(
            PollEndpoint, pw);

        LogDweet LD = new LogDweet();
        LD.url = _baseGetUrl + _dweetThing + pw.Id.ToString();
        LD.TimeStamp = DateTime.Now.ToUniversalTime();

        //Post Log
        return await _client.PostAsJsonAsync(
            _dweetThing + "Log", LD);


    }


}