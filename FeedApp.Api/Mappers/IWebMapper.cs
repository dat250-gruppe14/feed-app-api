using FeedApp.Common.Models.Entities;
using FeedApp.Common.Models.Messaging;
using FeedApp.Api.Models.Web;

namespace FeedApp.Api.Mappers;

public interface IWebMapper
{
    PollWeb MapPollToWeb(Poll poll, Guid? userId, bool countVotes = true);
    Poll MapPollCreateRequestToInternal(PollCreateRequest request);
    UserWeb MapUserToWeb(User user);
    User MapUserCreateRequestToInternal(UserCreateRequest request);
    User MapUserUpdateRequestToInternal(UserUpdateRequest request);
    VoteWeb MapVoteToWeb(Vote vote, Guid? userId);

    DeviceWeb MapDeviceToWeb(Device device, Guid? userId);
    Device MapDeviceCreateRequestToInternal(DeviceCreateRequest request);
    Device MapDeviceUpdateRequestToInternal(DeviceUpdateRequest request);
    DeviceVote MapDevicVoteCreateRequestToInternal(DeviceVoteCreateRequest request);
    PollPub MapPollToPublish(Poll poll);
}
