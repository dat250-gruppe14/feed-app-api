using AutoMapper;
using FeedApp.Common.Models.Entities;
using FeedApp.Common.Models.Messaging;
using FeedApp.Api.Models.Web;

namespace FeedApp.Api.Mappers;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        AllowNullCollections = true;

        CreateMap<Poll, PollWeb>();
        CreateMap<PollCreateRequest, Poll>();
        CreateMap<User, UserWeb>();
        CreateMap<UserCreateRequest, User>();
        CreateMap<UserUpdateRequest, User>();
        CreateMap<Vote, VoteWeb>();
        CreateMap<Device, DeviceWeb>();
        CreateMap<DeviceCreateRequest, Device>();
        CreateMap<DeviceUpdateRequest, Device>();
        CreateMap<DeviceVoteCreateRequest, DeviceVote>();
        CreateMap<Poll, PollPub>();
        CreateMap<User, UserPub>();
    }
}
