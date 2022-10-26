using FeedAppApi.Models;

namespace FeedAppApi.Services;

public interface IUserService
{
    public User addUser(CreateUserRequest user);
    public User? editUser(string UserID, CreateUserRequest user);
    public User? deleteUser(string UserID);
}