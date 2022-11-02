using AutoMapper;
using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;

namespace FeedAppApi.Mappers;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        AllowNullCollections = true;

        CreateMap<Poll, PollWeb>();
        CreateMap<PollCreateRequest, Poll>();
        CreateMap<User, UserWeb>();
        CreateMap<UserCreateRequest, User>();
    }
}