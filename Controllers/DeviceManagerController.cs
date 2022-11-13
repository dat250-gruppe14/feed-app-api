using FeedAppApi.Mappers;
using FeedAppApi.Models.Web;
using FeedAppApi.Services;
using FeedAppApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedAppApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[LoggedInUserFilter]
public class DeviceManagerController : ControllerBase
{
  private readonly ILogger<DeviceManagerController> _logger;
  private readonly IDeviceManagerService _deviceService;
  private readonly IAuthUtils _authUtils;
  private IWebMapper _webMapper;

  public DeviceManagerController(ILogger<DeviceManagerController> logger,
	  IDeviceManagerService deviceService,
	  IWebMapper webMapper,
	  IAuthUtils authUtils)
  {
	_logger = logger;
	_deviceService = deviceService;
	_webMapper = webMapper;
	_authUtils =authUtils;
  }


  [HttpGet(Name = "GetDevice")]
  public IActionResult GetDevicePoll()
  {
	var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
	var devices =  _deviceService.GetDevices(currentUser!.Id);

	return Ok(devices.Select(device => _webMapper.MapDeviceToWeb(device, currentUser!.Id)));
  }

  [HttpPost(Name = "CreateDevice")]
  public async Task<IActionResult> CreateDevice([FromBody] DeviceCreateRequest deviceCreateRequest)
  {
	var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
	var device = await _deviceService.CreateDevice(currentUser!,
		_webMapper.MapDeviceCreateRequestToInternal(deviceCreateRequest),
		deviceCreateRequest.PollPincode!);

	return Ok(_webMapper.MapDeviceToWeb(device, currentUser?.Id));
  }

  [HttpPatch("{deviceId}", Name = "UpdateDevice")]
  public async Task<IActionResult> UpdateDevice([FromRoute] string deviceId, [FromBody] DeviceUpdateRequest deviceUpdateRequest)
  {
	var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
	var device = await _deviceService.UpdateDevice(
		currentUser!, 
		Guid.Parse(deviceId), 
		deviceUpdateRequest.Name, 
		deviceUpdateRequest.PollPincode);

	return Ok(_webMapper.MapDeviceToWeb(device!, currentUser?.Id));
  }

  [HttpDelete("{deviceId}", Name = "DeleteDevice")]
  public async Task<IActionResult> DeleteDevice([FromRoute] string deviceId)
  {
	var currentUser = _authUtils.GetLoggedInUserFromHttpContext(HttpContext);
	var device = await _deviceService.DeleteDevice(currentUser!, Guid.Parse(deviceId));

	return Ok(_webMapper.MapDeviceToWeb(device!, currentUser?.Id));
  }
}
