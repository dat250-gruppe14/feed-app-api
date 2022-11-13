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

	public VoteWeb MapVoteToWeb(Vote vote, Guid? userId)
	{

		var pollWeb = this.MapPollToWeb(vote.Poll, userId);
		pollWeb.UserAnswer = vote.OptionSelected;


		var vWeb = new VoteWeb();
		vWeb.pollWeb = pollWeb;
		return vWeb;
	}

	public Poll MapDeviceCreateRequestToInternal(PollCreateRequest request)
	{
		return _automapper.Map<Poll>(request);
	}

	public DeviceWeb MapDeviceToWeb(Device device, Guid? userId)
	{
		var deviceWeb = _automapper.Map<DeviceWeb>(device);
		deviceWeb.ConnectedPoll = MapPollToWeb(device.ConnectedPoll, userId);

		return deviceWeb;
	}

	public Device MapDeviceCreateRequestToInternal(DeviceCreateRequest request)
	{
		return _automapper.Map<Device>(request);
	}

	public Device MapDeviceUpdateRequestToInternal(DeviceUpdateRequest request)
	{
		return _automapper.Map<Device>(request);
	}

    public DeviceVote MapDevicVoteCreateRequestToInternal(DeviceVoteCreateRequest request)
    {
		return _automapper.Map<DeviceVote>(request);
    }
}
