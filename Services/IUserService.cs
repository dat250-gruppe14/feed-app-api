using FeedAppApi.Models.Entities;

namespace FeedAppApi.Services;

public interface IUserService
{
    public Task<User> createUser(User user);
    public Task<User?> editUser(User currentUser, Guid Id, User newUser);
    public Task<User?> deleteUser(string UserID);
    public Task<Device> createDevice(Device device);
    public Task<Device?> editDevice(Guid Id, Device updatedDevice);
    public Task<Device?> deleteDevice(string deviceId);
    public Task<Device?> getDevices(Guid userId);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(Guid id);
    Task<User?> UpdateRefreshToken(Guid userId, string refreshToken);
    User? GetLoggedInUser(HttpContext httpContext);
}
