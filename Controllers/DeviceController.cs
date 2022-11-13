using FeedAppApi.Mappers;
using FeedAppApi.Models.Entities;
using FeedAppApi.Models.Web;
using FeedAppApi.Services;
using FeedAppApi.Utils;
using Microsoft.AspNetCore.Mvc;

namespace FeedAppApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[LoggedInDeviceFilter]
public class DeviceController : ControllerBase
{
    private readonly ILogger<DeviceController> _logger;
    private readonly IDeviceService _deviceService;
    private readonly IAuthUtils _authUtils;
    private IWebMapper _webMapper;

    public DeviceController(ILogger<DeviceController> logger, IDeviceService deviceService, IWebMapper webMapper, IAuthUtils authUtils)
    {
        _logger = logger;
        _deviceService = deviceService;
        _webMapper = webMapper;
        _authUtils =authUtils;
    }


    [HttpGet(Name = "GetDevicePoll")]
    public IActionResult GetDevicePoll()
    {
        var device = (Device) HttpContext.Items["device"]!;
		if(device is null){
		  return Unauthorized();
		}

        return Ok(_webMapper.MapDeviceToWeb(device, device.UserId));
    }

    [HttpPost(Name = "CreateDeviceVote")]
    public async Task<IActionResult> CreateDeviceVote([FromBody] DeviceVoteCreateRequest deviceCreateRequest)
    {
        var device = (Device) HttpContext.Items["device"]!;
		if(device is null){
		  return Unauthorized();
		}

        var deviceVote = await _deviceService.CreateDeviceVote(device, _webMapper.MapDevicVoteCreateRequestToInternal(deviceCreateRequest));

        return Ok(_webMapper.MapDeviceToWeb(deviceVote.Device!, deviceVote.Device!.UserId));
    }
}

