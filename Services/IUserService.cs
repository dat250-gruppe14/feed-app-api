using FeedAppApi.Models;

namespace FeedAppApi.Services;

public interface IUserService
{
    public User createUser(CreateUserRequest user);
    public User? editUser(string UserID, CreateUserRequest user);
    public User? deleteUser(string UserID);
}