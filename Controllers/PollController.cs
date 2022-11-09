using FeedAppApi.Exceptions;
using FeedAppApi.Mappers;
using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;
using FeedAppApi.Services;
using FeedAppApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace FeedAppApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[LoggedInUserFilter]
public class PollController : ControllerBase
{
    private readonly ILogger<PollController> _logger;
    private readonly IPollService _pollService;
    private readonly IWebMapper _webMapper;
    private readonly IAuthUtils _authUtils;

    public PollController(
	    ILogger<PollController> logger,
	    IPollService pollService,
	    IWebMapper webMapper,
	    IAuthUtils authUtils)
    {
        _logger = logger;
        _pollService = pollService;
        _webMapper = webMapper;
        _authUtils = authUtils;
    }

    [HttpGet(Name = "GetPolls")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPolls()
    {
	    var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
	    
		var polls = await _pollService.GetPolls(currentUser);
		return Ok(polls.Select(poll => _webMapper.MapPollToWeb(poll, currentUser?.Id)));
    }

    [HttpGet("{pincode}", Name = "GetPollByPincode")]
    public async Task<IActionResult> GetPollByPincode([FromRoute] string pincode)
    {
	    var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
	    
        var poll = await _pollService.GetPollByPincode(pincode);
        return poll != null
	        ? Ok(_webMapper.MapPollToWeb(poll, currentUser?.Id))
	        : ResponseUtils.NotFoundResponse($"Poll with pincode {pincode} doesn't exist");
    }

    [HttpPost(Name = "CreatePoll")]
    public async Task<IActionResult> CreatePoll([FromBody] PollCreateRequest pollCreateRequest)
    {
        var poll = await _pollService.CreatePoll(_webMapper.MapPollCreateRequestToInternal(pollCreateRequest));
        return Ok(_webMapper.MapPollToWeb(poll, null, false));
    }

    [HttpPatch("{pincode}", Name = "UpdatePoll")]
    [AllowAnonymous]
    public async Task<IActionResult> PatchPoll([FromRoute] string pincode, [FromBody] JsonPatchDocument<Poll> pollDocument)
    {
	    var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
	    
        var updatedPoll = await _pollService.PatchPoll(pincode, pollDocument);
        if (updatedPoll == null)
        {
	        return ResponseUtils.NotFoundResponse($"Poll with pincode {pincode} doesn't exist");
        }
        return Ok(_webMapper.MapPollToWeb(updatedPoll, currentUser?.Id));
    }

    [HttpDelete("{pincode}", Name = "DeletePollByPincode")]
    public async Task<IActionResult> DeletePollByPincode([FromRoute] string pincode)
    {
	    var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
	    
	    try
	    {
		    var poll = await _pollService.DeletePoll(pincode, currentUser?.Id);

		    if (poll == null)
		    {
			    return ResponseUtils.NotFoundResponse($"Poll with pincode {pincode} doesn't exist.");
		    }
		    
		    return Ok(_webMapper.MapPollToWeb(poll, null, false));
	    }
	    catch (NoAccessException ex)
	    {
		    return ResponseUtils.UnauthorizedResponse(ex.Message);
	    }
    }
    
}
