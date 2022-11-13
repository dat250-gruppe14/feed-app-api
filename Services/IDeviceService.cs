using FeedAppApi.Models.Entities;

namespace FeedAppApi.Services;

public interface IDeviceService {
	public Task<DeviceVote> CreateDeviceVote(Device device, DeviceVote deviceVote);
	public Device? GetLoggedInDevice(HttpContext httpContext);
	public bool IsCorrectDeviceCredentials(Device device, string token);
}

