using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;




using FeedAppApi.Mappers;
using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;
using FeedAppApi.Utils;
using FeedAppApi.Proxies.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FeedAppApi.Services;

public class DweetMessagingService : IDweetMessagingService
{

    private string _baseurl = "https://dweet.io/dweet/for/";
    private string _dweetThing = "G14FeedApp";
    private string _urlPost;
    private string _urlContent = "https://dweet.io/get/latest/dweet/for/";

    private readonly IPollUtils _pollUtils;
    private List<Guid> _postedPollIds;

    private HttpClient _client;
    private bool _isInited;



    private readonly IWebMapper _webMapper;

    public DweetMessagingService(IWebMapper webMapper, IPollUtils pollUtils)
    {
        _webMapper = webMapper;
        _urlPost = _baseurl + _dweetThing;
        _pollUtils = pollUtils;
        _postedPollIds = new List<Guid>();
        _client = new()
        {
            BaseAddress = new Uri(_urlPost),
        };
        _isInited = false;

    }

    public Guid? GetNextGuid(Guid id)
    {
        for (int i = 0; i < _postedPollIds.Count; i++)
        {
            var pw = _postedPollIds[i];
            if (pw == id)
            {
                return _postedPollIds.Count > (i + 1) ? _postedPollIds[i + 1] : null;
            }
        }

        return null;
    }

    //Posts all polls in the database and loads the _postedPollIds list
    public async Task<HttpResponseMessage> init(DataContext context)
    {
        var polls = await _pollUtils.GetAllPolls(context);

        var pollWebs = polls.Select(poll => _webMapper.MapPollToWeb(poll, null, false)).ToList();
        PollMessageDweet nextMessage = new PollMessageDweet();
        

        List<string> urlUpdated = new List<String>();

        for (var i = 0; i < pollWebs.Count; i++)
        {
            nextMessage = new PollMessageDweet();
            nextMessage.pollWeb = pollWebs[i];
            
            
            string link = _urlPost + i.ToString();

            if (i + 1 < pollWebs.Count)
            {
                nextMessage.nextPoll = _urlContent + _dweetThing + pollWebs[i+1].Id.ToString();
            }
;
            _postedPollIds.Add(nextMessage.pollWeb.Id);
            urlUpdated.Add(link);

            await this.PostPollDweet(link, nextMessage);
            

        }

        UpdateInfoDweet iDweet = new UpdateInfoDweet();
        iDweet.NewlyUpdatedPolls = urlUpdated;
        return await this.PostUpdateInfoDweet(iDweet);

    }

    //Update an already posted poll
    public async Task<HttpResponseMessage> PostUpdate(Poll poll, DataContext context)
    {
        if (!_isInited)
        {
            return await this.init(context);
            _isInited = true;
        }

        PollMessageDweet mDweet = new PollMessageDweet();
        mDweet.nextPoll = this.GetNextGuid(poll.Id).ToString();
        mDweet.pollWeb = _webMapper.MapPollToWeb(poll, null, false);

        

        List<string> updatedUrl = new List<string>();
        updatedUrl.Add(_urlContent + poll.Id.ToString());
        UpdateInfoDweet iDweet = new UpdateInfoDweet();
        iDweet.NewlyUpdatedPolls = updatedUrl;
        await this.PostUpdateInfoDweet(iDweet);

        return await this.PostPollDweet(_urlPost + poll.Id.ToString(), mDweet);

    }
    
    //Posts a new poll
    public async Task<HttpResponseMessage> PostNew(Poll poll, DataContext context)
    {
        if (!_isInited)
        {
            return await this.init(context);
            _isInited = true;
        }
        PollMessageDweet newDweet = new PollMessageDweet();
        newDweet.pollWeb = _webMapper.MapPollToWeb(poll, null, false);
        string id = poll.Id.ToString();

        PollMessageDweet oldLastDweet = new PollMessageDweet();
        var oldLastIds = _postedPollIds[-1];
        Poll oldLastPoll = context.Polls.Single(p => p.Id.Equals(_postedPollIds));
        oldLastDweet.pollWeb = _webMapper.MapPollToWeb(oldLastPoll, null, false);
        oldLastDweet.nextPoll = _urlContent + _dweetThing + id;

        List<string> updatedUrl = new List<string>();
        updatedUrl.Add(_urlContent + poll.Id);
        UpdateInfoDweet iDweet = new UpdateInfoDweet();
        iDweet.NewlyUpdatedPolls = updatedUrl;
        this.PostUpdateInfoDweet(iDweet);

        await this.PostPollDweet(_urlPost + oldLastIds.ToString(), oldLastDweet);
        return await this.PostPollDweet(_urlPost + id, newDweet);



    }


    //Posts a Poll to Dweet
    public async Task<HttpResponseMessage> PostPollDweet(string dweeturl, PollMessageDweet md)
    {
        _client = new()
        {
            BaseAddress = new Uri(dweeturl),
        };

        return await _client.PostAsJsonAsync(
            "", md);
    }


    //Post a dweet which is dedicated to all new changes
    public async Task<HttpResponseMessage> PostUpdateInfoDweet(UpdateInfoDweet updated)
    {
        updated.UpdateTime = DateTime.Now.ToUniversalTime();
        updated.FirstPoll = _urlContent + _postedPollIds[0].ToString();
        _client = new()
        {
            BaseAddress = new Uri(_urlPost + "updates"),
        };

        return await _client.PostAsJsonAsync(
            "", updated);
    }
}