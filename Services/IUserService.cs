using FeedAppApi.Models.Web;
using FeedAppApi.Models.Entities;


namespace FeedAppApi.Services;

public interface IUserService
{
    public Task<User> createUser(User user);
    public Task<User?> editUser(Guid Id, User newUser);
    public Task<User?> deleteUser(string UserID);
}