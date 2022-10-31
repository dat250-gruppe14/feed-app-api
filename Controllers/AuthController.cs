using FeedAppApi.Models.Web;
using FeedAppApi.Utils;
using Microsoft.AspNetCore.Mvc;

namespace FeedAppApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthUtils _authUtils;

    public AuthController(IAuthUtils authUtils)
    {
        _authUtils = authUtils;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] CredetialsWeb credetials)
    {
        var userId = Guid.NewGuid(); // TODO: Get userId if credentials match db
        return Ok(_authUtils.GenerateToken(userId));
    }

}