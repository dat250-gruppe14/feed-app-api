
using FeedApp.Api.Proxies.Data;
using FeedApp.Api.Utils;
using FeedApp.Common.Models.Entities;

namespace FeedApp.Api.Services;

public class DeviceService : IDeviceService
{
	private readonly IAuthUtils _authUtils;
	private readonly IPasswordUtils _passwordUtils;
	private readonly DataContext _context;

    public DeviceService(IAuthUtils authUtils, DataContext context, IPasswordUtils passwordUtils)
    {
		_authUtils = authUtils;
		_context = context;
		_passwordUtils = passwordUtils;
    }

    public async Task<DeviceVote> CreateDeviceVote(Device device, DeviceVote deviceVote)
    {
		deviceVote.DeviceId = device.Id;
		deviceVote.PollId = device.PollId;
		var vote = _context.DeviceVotes.Add(deviceVote);
		await _context.SaveChangesAsync();
		return vote.Entity;
    }

	public bool IsCorrectDeviceCredentials(Device device, string token){
	  return _passwordUtils.VerifyPassword(token, device.salt!, device.hashedConnectionKey!);
	}

    public Device? GetLoggedInDevice(HttpContext httpContext)
    {
        var token = _authUtils.GetTokenFromHttpContext(httpContext);
        if (token == null) return null;
		var deviceId = httpContext.Request.Headers.First(h => h.Key == "Id").Value.ToString();
        if (deviceId == null) return null;
        
        return _context.Devices.Where(u => u.Id == Guid.Parse(deviceId))
            .FirstOrDefault();
    }

}
