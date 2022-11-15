using System.Net;
using FeedApp.Api.Errors;
using FeedApp.Api.Mappers;
using FeedApp.Api.Models.Web;
using FeedApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using FeedApp.Api.Utils;
using FeedApp.Common.Exceptions;

namespace FeedApp.Api.Controllers;

[ApiController]
[Route("api/poll/{pincode}/vote")]
[LoggedInUserFilter]
public class VoteController : ControllerBase 
{

    private readonly ILogger<VoteController> _logger;
    private readonly IVoteService _voteService;
    private readonly IWebMapper _webMapper;
    private readonly IAuthUtils _authUtils;

    public VoteController(
        ILogger<VoteController> logger, 
        IVoteService voteService, 
        IWebMapper webMapper,
        IAuthUtils authUtils)
    {
        _logger = logger;
        _voteService = voteService;
        _webMapper = webMapper;
        _authUtils = authUtils;
    }

    [HttpPost(Name = "CreateVote")]
    public async Task<IActionResult> CreateVote([FromRoute] string pincode, [FromBody] VoteCreateRequest req)
    {
        var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
        req.PollPincode = pincode;
        try
        {
            var vote = await _voteService.createVote(currentUser, req);
            return Ok(_webMapper.MapPollToWeb(vote.Poll, currentUser?.Id));
        }
        catch (NotFoundException e)
        {
            return ResponseUtils.NotFoundResponse(e.Message);
        }
        catch (EfCoreException e)
        {
            return BadRequest(new ApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Message = e.Message
            });
        }
        catch (NoAccessException e)
        {
            return ResponseUtils.UnauthorizedResponse(e.Message);
        }
        catch (NotAllowedException e)
        {
            return BadRequest(new ApiErrorResponse
            {
                Status = HttpStatusCode.BadRequest,
                Message = e.Message
            });
        }
    }

}