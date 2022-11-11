using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;




using FeedAppApi.Mappers;
using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;

namespace FeedAppApi.Services;

public class DweetMessagingService : IDweetMessagingService
{

    private string _baseurl = "https://dweet.io/dweet/for/";
    private string _dweetThing = "FeedAppG14";
    private string _url;

    private HttpClient _client;

    private static readonly JsonSerializerOptions jsonSerializerOptions
        = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

    private readonly IWebMapper _webMapper;

    public DweetMessagingService(IWebMapper webMapper)
    {
        _webMapper = webMapper;
        _url = _baseurl + _dweetThing;
        _client = new()
        {
            BaseAddress = new Uri(_url),
        };
        
    }

    public async Task<HttpResponseMessage> Post(Poll poll)
    {
        //poll.userAnswer = null;
        //var pollWebs = polls.Select(poll => _webMapper.MapPollToWeb(poll, null, false));
        var pollWeb = _webMapper.MapPollToWeb(poll, null, false);
        using HttpResponseMessage response = await _client.PostAsJsonAsync(
            "", pollWeb);

        return response;
    }
}