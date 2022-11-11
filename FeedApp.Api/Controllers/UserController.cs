using FeedApp.Api.Errors;
using FeedApp.Api.Mappers;
using FeedApp.Api.Models.Web;
using FeedApp.Api.Services;
using FeedApp.Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace FeedApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[LoggedInUserFilter]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private IWebMapper _webMapper;
    private readonly IAuthUtils _authUtils;

    

    public UserController(
        ILogger<UserController> logger, 
        IUserService userService, 
        IWebMapper webMapper,
        IAuthUtils authUtils)
    {
        _logger = logger;
        _userService = userService;
        _webMapper = webMapper;
        _authUtils = authUtils;
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);

        var users = await _userService.GetAllUsers(currentUser);
        return users != null
            ? Ok(users.Select(user => _webMapper.MapUserToWeb(user)))
            : ResponseUtils.UnauthorizedResponse("You're not an admin.");
    }

    [HttpPut("{Id}", Name = "EditUser")]
    public async Task<IActionResult> EditUser([FromRoute] Guid Id, [FromBody] UserUpdateRequest newUser)
    {
        var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
        
        var user = await _userService.editUser(currentUser, Id,
            _webMapper.MapUserUpdateRequestToInternal(newUser));

        return Ok(_webMapper.MapUserToWeb(user));
    }

}