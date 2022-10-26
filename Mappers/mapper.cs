using AutoMapper;
using FeedAppApi.Models;

var configuration = new MapperConfiguration(cfg => 
{
    cfg.CreateMap<CreatePollRequest, Poll>();
});

// only during development, validate your mappings; remove it before release
#if DEBUG
configuration.AssertConfigurationIsValid();
#endif

// use DI (http://docs.automapper.org/en/latest/Dependency-injection.html) or create the mapper yourself
var mapper = configuration.CreateMapper();
