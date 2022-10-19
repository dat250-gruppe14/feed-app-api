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

    [HttpGet(Name = "GetPollById")]
    public Poll GetPollById()
    {
	var poll = _pollService.getPollById("a");
	return poll;
    }
}
