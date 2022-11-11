using FeedApp.Common.Models.Entities;

namespace FeedApp.Api.Services;

public interface IUserService
{
    public Task<User> createUser(User user);
    public Task<User?> editUser(User currentUser, Guid Id, User newUser);
    public Task<User?> deleteUser(string UserID);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(Guid id);
    Task<User?> UpdateRefreshToken(Guid userId, string refreshToken);
    User? GetLoggedInUser(HttpContext httpContext);
    Task<IEnumerable<User>?> GetAllUsers(User? user);
}