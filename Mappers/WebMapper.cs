using AutoMapper;
using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;
using FeedAppApi.Utils;

namespace FeedAppApi.Mappers;

public class WebMapper : IWebMapper
{
    private readonly IMapper _automapper;
    private readonly IPollUtils _pollUtils;

    public WebMapper(IMapper automapper, IPollUtils pollUtils)
    {
        _automapper = automapper;
        _pollUtils = pollUtils;
    }
    
    public PollWeb MapPollToWeb(Poll poll, Guid userId)
    {
        var pollWeb = _automapper.Map<PollWeb>(poll);
        
        var pollStats = _pollUtils.CountPollVotes(poll, userId);
        
        pollWeb.Counts = new PollCountsWeb {OptionOneCount = pollStats.OptionOneCount, OptionTwoCount = pollStats.OptionTwoCount};
        pollWeb.UserAnswer = pollStats.UserAnswer;
        
        return pollWeb;
    }

    public Poll MapPollCreateRequestToInternal(PollCreateRequest request)
    {
        return _automapper.Map<Poll>(request);
    }

    public User MapUserCreateRequestToInternal(UserCreateRequest request)
    {
        return _automapper.Map<User>(request);
    }
}