using FeedAppApi.Models;
using FeedAppApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeedAppApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PollController : ControllerBase
{
    private readonly ILogger<PollController> _logger;
    private readonly IPollService _pollService;

    public PollController(ILogger<PollController> logger, IPollService pollService)
    {
        _logger = logger;
	_pollService = pollService;	
    }

    [HttpGet(Name = "GetPolls")]
    public IEnumerable<Poll> GetPolls()
    {
	var poll = _pollService.getPolls();
	return poll?.ToList() ?? Enumerable.Empty<Poll>();
    }

    [HttpGet("{id}", Name = "GetPollById")]
    public Poll? GetPollById([FromRoute] string id)
    {
	var poll = _pollService.getPollById(id);
	return poll;
    }

    [HttpPost(Name = "createPoll")]
    public Poll CreatePoll([FromBody] CreatePollRequest createPollRequest)
    {
	var poll = _pollService.createPoll(createPollRequest);
	return poll;
    }
}
