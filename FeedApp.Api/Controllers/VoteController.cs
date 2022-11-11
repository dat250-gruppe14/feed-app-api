using System;
using System.Net;
using System.Threading.Tasks;
using FeedApp.Api.Errors;
using FeedApp.Api.Mappers;
using FeedApp.Api.Models.Web;
using FeedApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using FeedApp.Api.Utils;
using FeedApp.Common.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using FeedApp.Common.Exceptions;

namespace FeedAppApi.Controllers;

[ApiController]
[Route("api/poll/{pincode}/vote")]
[LoggedInUserFilter]
public class VoteController : ControllerBase 
{

    private readonly ILogger<VoteController> _logger;
    private readonly IVoteService _voteService;
    private IWebMapper _webMapper;
    private IAuthUtils _authUtils;

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
            return Ok(_webMapper.MapVoteToWeb(vote, currentUser?.Id));
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
    }

      
        
    

}