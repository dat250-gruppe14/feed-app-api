using System;
using System.Net;
using System.Threading.Tasks;
using FeedApp.Api.Errors;
using FeedApp.Api.Mappers;
using FeedApp.Api.Models.Web;
using FeedApp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FeedApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private IWebMapper _webMapper;

    

    public UserController(ILogger<UserController> logger, IUserService userService, IWebMapper webMapper)
    {
        _logger = logger;
        _userService = userService;
        _webMapper = webMapper;
    }


    [HttpPost(Name = "CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest userCreateRequest)
    {
        var user = await _userService.createUser(_webMapper.MapUserCreateRequestToInternal(userCreateRequest));

        return Ok(user);
    }

    [HttpPut("{Id}", Name = "EditUser")]
    public async Task<IActionResult> EditUser([FromRoute] Guid Id,
                                            [FromBody] UserUpdateRequest newUser)
    {
        
        var user = await _userService.editUser(Id,
            _webMapper.MapUserUpdateRequestToInternal(newUser));

        return Ok(user);
    }

}