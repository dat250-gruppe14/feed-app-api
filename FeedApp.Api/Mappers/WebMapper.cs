using AutoMapper;
using FeedApp.Common.Models.Entities;
using FeedApp.Common.Models.Messaging;
using FeedApp.Api.Models.Web;
using FeedApp.Api.Utils;

namespace FeedApp.Api.Mappers;

public class WebMapper : IWebMapper
{
    private readonly IMapper _automapper;
    private readonly IPollUtils _pollUtils;

    public WebMapper(IMapper automapper, IPollUtils pollUtils)
    {
        _automapper = automapper;
        _pollUtils = pollUtils;
    }
    
    public PollWeb MapPollToWeb(Poll poll, Guid? userId , bool countVotes = true)
    {
        var pollWeb = _automapper.Map<PollWeb>(poll);

        if (!countVotes)
        {
            pollWeb.Counts = new PollCountsWeb();
            return pollWeb;
        }
        
        var pollStats = _pollUtils.CountPollVotes(poll, userId);
        pollWeb.Counts = new PollCountsWeb {OptionOneCount = pollStats.OptionOneCount, OptionTwoCount = pollStats.OptionTwoCount};
        pollWeb.UserAnswer = pollStats.UserAnswer;

        return pollWeb;
    }

    public Poll MapPollCreateRequestToInternal(PollCreateRequest request)
    {
        return _automapper.Map<Poll>(request);
    }

    public UserWeb MapUserToWeb(User user)
    {
        return _automapper.Map<UserWeb>(user);
    }
    
    public User MapUserCreateRequestToInternal(UserCreateRequest request)
    {
        return _automapper.Map<User>(request);
    }

    public User MapUserUpdateRequestToInternal(UserUpdateRequest request)
    {
        return _automapper.Map<User>(request);
    }

    public PollWeb MapVoteToWeb(Vote vote, Guid? userId)
    {
        
        var pollWeb = MapPollToWeb(vote.Poll, userId);
        pollWeb.UserAnswer = vote.OptionSelected;
        
        return pollWeb;
    }

    public PollPub MapPollToPublish(Poll poll)
    {
        return _automapper.Map<PollPub>(poll);
    }
}