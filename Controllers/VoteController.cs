using System;
using System.Net;
using System.Threading.Tasks;
using FeedAppApi.Errors;
using FeedAppApi.Mappers;
using FeedAppApi.Models.Web;
using FeedAppApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeedAppApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class VoteController : ControllerBase 
{

    private readonly ILogger<VoteController> _logger;
    private readonly IVoteService _voteService;
    private IWebMapper _webMapper;



    public VoteController(ILogger<VoteController> logger, IVoteService voteService, IWebMapper webMapper)
    {
        _logger = logger;
        _voteService = voteService;
        _webMapper = webMapper;
    }

    [HttpPost("{Id}", Name = "CreateVote")]
    public async Task<IActionResult> CreateVote([FromRoute] Guid Id,
        [FromBody] VoteCreateRequest req)
    {
        var vote = await _voteService.createVote(Id, req);


        return Ok(vote);
    }

}