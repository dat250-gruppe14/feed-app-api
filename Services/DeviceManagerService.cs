using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Utils;

namespace FeedAppApi.Services;

public class DeviceManagerService : IDeviceManagerService
{
  private readonly DataContext _context;
  private readonly IUserService _userService;
  private readonly IPollService _pollService;
  private readonly IAuthUtils _authUtils;
  private readonly IPasswordUtils _passwordUtils;

  public DeviceManagerService(DataContext context,
	  IPasswordUtils passwordUtils,
	  IPollService pollService,
	  IAuthUtils authUtils,
	  IUserService userService)
  {
	_context = context;
	_passwordUtils = passwordUtils;
	_userService = userService;
	_authUtils = authUtils;
	_pollService = pollService;
  }

  public async Task<Device> CreateDevice(User authUser, Device device, string pincode)
  {
	var poll = await _pollService.GetPollByPincode(pincode);
	device.isActive = true;
	device.UserId = authUser.Id!;
	device.PollId = poll!.Id;

	var generatedConnectionToken = CreateAndReturnNewDeviceToken(device);

	var createdDevice = _context.Devices.Add(device);
	await _context.SaveChangesAsync();
	var deviceResponse = createdDevice.Entity;
	deviceResponse.connectionToken = generatedConnectionToken;
	return deviceResponse;
  }

  public bool IsCorrectDeviceCredentials(Device device, string token){
	return _passwordUtils.VerifyPassword(device.hashedConnectionKey!, device.salt!, token);
  }

  public async Task<Device?> DeleteDevice(User authUser, Guid? deviceId)
  {
	var deviceToDelete = GetDevices(authUser.Id).First(device => device.Id == deviceId);
	deviceToDelete.isActive = false;
	_context.Devices.Update(deviceToDelete);
	await _context.SaveChangesAsync();
	return deviceToDelete;
  }

  public IEnumerable<Device> GetDevices(Guid userId)
  {
	return _context.Devices.AsEnumerable()
	  .Where(device => device.isActive ?? false)
	  .Where(device => device.UserId == userId)
	  .ToList();
  }
  public async Task<Device?> UpdateDevice(User authUser, Guid? deviceId, string? deviceName, string? pincode)
  {
	var deviceToUpdate = GetDevices(authUser.Id).First(device => device.Id == deviceId);
	if (deviceToUpdate == null)
	{
		return deviceToUpdate;
	}
	deviceToUpdate.Name = deviceName ?? deviceToUpdate.Name;
	if (!(pincode is null)){
	  var poll = await _pollService.GetPollByPincode(pincode);
	  if(!(poll is null)){
		deviceToUpdate.PollId = poll!.Id;
	  }
	}
	_context.Update(deviceToUpdate);
	await _context.SaveChangesAsync();

	return deviceToUpdate;
  }

  public async Task<Device?> UpdateDeviceToken(User authUser, Guid? deviceId)
  {
	var deviceToUpdate = GetDevices(authUser.Id).First(device => device.Id == deviceId);
	var generatedConnectionToken = CreateAndReturnNewDeviceToken(deviceToUpdate);

	var updatedDevice = _context.Update(deviceToUpdate).Entity;
	await _context.SaveChangesAsync();

	updatedDevice.connectionToken = generatedConnectionToken;

	return updatedDevice;
  }

  private string CreateAndReturnNewDeviceToken(Device device){
	var generatedConnectionToken = GenerateToken();
	var salt = _passwordUtils.GenerateSalt();
	var hash = _passwordUtils.HashPassword(generatedConnectionToken, salt);

	device.hashedConnectionKey = hash;
	device.salt = salt;

	return generatedConnectionToken;
  }

  private string GenerateToken(){
	return _authUtils.GenerateRefreshToken();
  }
}
