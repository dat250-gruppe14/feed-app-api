using System.Net;
using FeedApp.Api.Errors;
using FeedApp.Api.Mappers;
using FeedApp.Api.Models.Web;
using FeedApp.Api.Services;
using FeedApp.Api.Utils;
using FeedApp.Common.Exceptions;
using FeedApp.Common.Enums;
using FeedApp.Common.Models.Entities;
using FeedApp.Messaging.Sender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace FeedApp.Api.Controllers;

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
    private readonly IPollExpiredSender _pollExpiredSender;

    public PollController(
	    ILogger<PollController> logger,
	    IPollService pollService,
	    IWebMapper webMapper,
	    IAuthUtils authUtils,
        IPollExpiredSender pollExpiredSender)
    {
        _logger = logger;
        _pollService = pollService;
        _webMapper = webMapper;
        _authUtils = authUtils;
        _pollExpiredSender = pollExpiredSender;
    }

    [HttpGet(Name = "GetPolls")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPolls()
    {
	    var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
	    
		var polls = await _pollService.GetPolls(currentUser);
		return Ok(polls.Select(poll => _webMapper.MapPollToWeb(poll, currentUser?.Id)));
    }

    [AllowAnonymous]
    [HttpGet("{pincode}", Name = "GetPollByPincode")]
    public async Task<IActionResult> GetPollByPincode([FromRoute] string pincode)
    {
	    var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
	    
        var poll = await _pollService.GetPollByPincode(pincode);
        if (poll == null)
	        return ResponseUtils.NotFoundResponse($"Poll with pincode {pincode} doesn't exist");

        if (currentUser == null && poll.Access == PollAccess.Private)
        {
	        return ResponseUtils.UnauthorizedResponse("Log in to access private polls.");
        }

        return Ok(_webMapper.MapPollToWeb(poll, currentUser?.Id));
    }

    [HttpPost(Name = "CreatePoll")]
    public async Task<IActionResult> CreatePoll([FromBody] PollCreateRequest pollCreateRequest)
    {
	    try
	    {
			var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
	        var poll = await _pollService.CreatePoll(_webMapper.MapPollCreateRequestToInternal(pollCreateRequest), currentUser);
            _pollExpiredSender.SendPoll(_webMapper.MapPollToPublish(poll));
	        return Ok(_webMapper.MapPollToWeb(poll, null, false));
	    }
	    catch (EfCoreException e)
	    {
		    return StatusCode(500, new ApiErrorResponse
		    {
			    Status = HttpStatusCode.InternalServerError,
			    Message = e.Message
		    });
	    }
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
		    var poll = await _pollService.DeletePoll(pincode, currentUser);

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
