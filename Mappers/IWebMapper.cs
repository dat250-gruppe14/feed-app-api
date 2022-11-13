using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;

namespace FeedAppApi.Mappers;

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
}
