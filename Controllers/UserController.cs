using FeedAppApi.Mappers;
using FeedAppApi.Models.Web;
using FeedAppApi.Services;
using Microsoft.AspNetCore.Mvc;
using FeedAppApi.Utils;


namespace FeedAppApi.Controllers;

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


    [HttpPut("{Id}", Name = "EditUser")]
    public async Task<IActionResult> EditUser([FromRoute] Guid Id, [FromBody] UserUpdateRequest newUser)
    {
        var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
        
        var user = await _userService.editUser(currentUser, Id,
            _webMapper.MapUserUpdateRequestToInternal(newUser));

        return Ok(user);
    }

}
