using AutoMapper;
using FeedAppApi.Models;

public class PollProfile : Profile {
	public PollProfile() {
		CreateMap<CreatePollRequest, Poll>();
	}
}
