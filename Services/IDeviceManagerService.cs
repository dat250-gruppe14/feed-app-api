using FeedAppApi.Models.Entities;

namespace FeedAppApi.Services;

public interface IDeviceManagerService {
	public IEnumerable<Device> GetDevices(Guid userId);
	public Task<Device> CreateDevice(User authUser, Device device, string pincode);
	public Task<Device?> DeleteDevice(User authUser, Guid? deviceId);
	public Task<Device?> UpdateDevice(User authUser, Guid? deviceId, string? deviceName, string? pincode);
	public Task<Device?> UpdateDeviceToken(User authUser, Guid? deviceId);
}
