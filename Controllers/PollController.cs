using System.Net;
using FeedAppApi.Errors;
using FeedAppApi.Exceptions;
using FeedAppApi.Mappers;
using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;
using FeedAppApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace FeedAppApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PollController : ControllerBase
{
    private readonly ILogger<PollController> _logger;
    private readonly IPollService _pollService;
    private readonly IWebMapper _webMapper;

    // Temporary. UserId should come from JWT token?
    private readonly Guid TEMP_USER = Guid.Parse("88d1bef4-3a48-4476-8b20-2662cb795c9a");

    public PollController(ILogger<PollController> logger, IPollService pollService, IWebMapper webMapper)
    {
        _logger = logger;
        _pollService = pollService;
        _webMapper = webMapper;
    }

    [HttpGet(Name = "GetPolls")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPolls()
    {
		var polls = await _pollService.GetPolls(new User {Id = TEMP_USER});
		// var polls = await _pollService.GetPolls(null);
		return Ok(polls.Select(poll => _webMapper.MapPollToWeb(poll, TEMP_USER)));
    }

    [HttpGet("{pincode}", Name = "GetPollByPincode")]
    public async Task<IActionResult> GetPollByPincode([FromRoute] string pincode)
    {
        var poll = await _pollService.GetPollByPincode(pincode);
        return poll != null
            // TODO: Get userId from header and throw into mapper
            ? Ok(_webMapper.MapPollToWeb(poll, TEMP_USER))
            : NotFound(new ApiErrorResponse
            {
                Status = HttpStatusCode.NotFound,
                Message = $"Poll with pincode {pincode} doesn't exist"
            });
    }

    [HttpPost(Name = "CreatePoll")]
    public async Task<IActionResult> CreatePoll([FromBody] PollCreateRequest pollCreateRequest)
    {
        var poll = await _pollService.CreatePoll(_webMapper.MapPollCreateRequestToInternal(pollCreateRequest));
        return Ok(poll);
    }

    [HttpPatch("{pincode}", Name = "UpdatePoll")]
    [AllowAnonymous]
    public async Task<IActionResult> PatchPoll([FromRoute] string pincode, [FromBody] JsonPatchDocument<Poll> pollDocument)
    {
        var updatedPoll = await _pollService.PatchPoll(pincode, pollDocument);
        if (updatedPoll == null)
        {
            return NotFound();
        }
        return Ok(_webMapper.MapPollToWeb(updatedPoll, TEMP_USER));
    }

    [HttpDelete("{pincode}", Name = "DeletePollByPincode")]
    public async Task<IActionResult> DeletePollByPincode([FromRoute] string pincode)
    {
	    try
	    {
		    var poll = await _pollService.DeletePoll(pincode, new User {Id = TEMP_USER});
		    return Ok(poll);
	    }
	    catch (NoAccessException ex)
	    {
		    return Unauthorized(new ApiErrorResponse
		    {
				Status = HttpStatusCode.Unauthorized,
				Message = ex.Message
		    });
	    }
	    catch (NotFoundException ex)
	    {
		    return NotFound(new ApiErrorResponse
		    {
			    Status = HttpStatusCode.NotFound,
			    Message = ex.Message
		    });
	    }
    }
    
}
