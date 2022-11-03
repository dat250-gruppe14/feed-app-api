using System.Net;
using FeedAppApi.Errors;
using FeedAppApi.Mappers;
using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;
using FeedAppApi.Services;
using FeedAppApi.Utils;
using Microsoft.AspNetCore.Mvc;

namespace FeedAppApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthUtils _authUtils;
    private readonly IUserService _userService;
    private readonly IWebMapper _webMapper;
    private readonly IPasswordUtils _passwordUtils;

    public AuthController(
        IAuthUtils authUtils,
        IUserService userService,
        IWebMapper webMapper,
        IPasswordUtils passwordUtils
    )
    {
        _authUtils = authUtils;
        _userService = userService;
        _webMapper = webMapper;
        _passwordUtils = passwordUtils;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginWeb credetials)
    {
        var user = await _userService.GetUserByEmail(credetials.Email);
        if (user == null || !_passwordUtils.VerifyPassword(credetials.Password, user.PasswordSalt, user.PasswordHash))
        {
            return Unauthorized(new ApiErrorResponse
            {
                Message = "Incorrect email and/or password",
                Status = HttpStatusCode.Unauthorized
            });
        }

        var token = await GenerateTokensAndSetCookies(user.Id, HttpContext);
        return Ok(new UserWithTokenWeb {User = _webMapper.MapUserToWeb(user), Token = token});
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterWeb credetials)
    {
        var existingUser = await _userService.GetUserByEmail(credetials.Email);
        if (existingUser != null)
        {
            return BadRequest(new ApiErrorResponse
            {
                Message = $"User with email {credetials.Email} already exist.",
                Status = HttpStatusCode.BadRequest
            });
        }

        var salt = _passwordUtils.GenerateSalt();
        var hash = _passwordUtils.HashPassword(credetials.Password, salt);

        var user = new User
        {
            Email = credetials.Email,
            Name = credetials.Name,
            PasswordHash = hash,
            PasswordSalt = salt
        };

        var createdUser = await _userService.createUser(user);
        
        var token = await GenerateTokensAndSetCookies(createdUser.Id, HttpContext);
        return Ok(new UserWithTokenWeb {User = _webMapper.MapUserToWeb(createdUser), Token = token});
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        string? token;
        try
        {
            token = HttpContext.Request.Headers.Authorization[0][7..];
        }
        catch (IndexOutOfRangeException)
        {
            token = null;
        }
        
        var refreshToken = HttpContext.Request.Cookies["refreshToken"];
        
        if (refreshToken == null || token == null)
        {
            return BadRequest(new ApiErrorResponse
            {
                Message = "Refresh token cookie and jwt token are required",
                Status = HttpStatusCode.BadRequest
            });
        }

        var tokenIsValid = _authUtils.ValidateExpiredToken(token);

        if (!tokenIsValid)
        {
            return Unauthorized(new ApiErrorResponse
            {
                Message = "Token is not valid",
                Status = HttpStatusCode.Unauthorized
            });
        }

        var userId = _authUtils.GetUserIdFromToken(token);
        var userIdParsed = Guid.Parse(userId);
        
        var user = await _userService.GetUserById(userIdParsed);

        if (user == null)
        {
            return Unauthorized(new ApiErrorResponse
            {
                Message = "JWT Token not verified",
                Status = HttpStatusCode.Unauthorized
            });
        }

        if (user.RefreshToken != refreshToken || user.RefreshTokenExpires < DateTime.UtcNow)
        {
            return Unauthorized(new ApiErrorResponse
            {
                Message = "Refresh token is invalid or has expired",
                Status = HttpStatusCode.Unauthorized
            });
        }

        var newToken = await GenerateTokensAndSetCookies(user.Id, HttpContext);
        return Ok(new UserWithTokenWeb {User = _webMapper.MapUserToWeb(user), Token = newToken});
    }

    private async Task<string> GenerateTokensAndSetCookies(Guid userId, HttpContext httpContext)
    {
        var token = _authUtils.GenerateToken(userId);
        var refreshToken = _authUtils.GenerateRefreshToken();

        await _userService.UpdateRefreshToken(userId, refreshToken);
        
        httpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true
        });

        return token;
    }
    
}