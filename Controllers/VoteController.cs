using System;
using System.Net;
using System.Threading.Tasks;
using FeedAppApi.Errors;
using FeedAppApi.Mappers;
using FeedAppApi.Models.Web;
using FeedAppApi.Services;
using Microsoft.AspNetCore.Mvc;
using FeedAppApi.Utils;
using FeedAppApi.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using FeedAppApi.Exceptions;

namespace FeedAppApi.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<IActionResult> CreateVote([FromBody] VoteCreateRequest req)
    {
        var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
        var vote = await _voteService.createVote(currentUser, req);

        if (currentUser == null)
        {
            return Ok(_webMapper.MapVoteToWeb(vote, null));
        }
        else
        {
            return Ok(_webMapper.MapVoteToWeb(vote, currentUser.Id));
        }

        
    }

      
        
    

}