using FeddBackApi.Models;

namespace FeedAppApi.Services;



public class UserService : IUserService
{
    private readonly DataContext _context;

    public UserService(DataContext context)
    {
        _context = context;
    }

    public async Task<User> createUser(UserCreateRequest req)
    {
        _context.DataBase.EnsureCreated();

        var user = new User();
        //Vet ikke vi kan generere id-en slik
        user.id = Guid.NewGuid().ToString();

        user.Name = req.Name;
        user.isAdmin = false;


        var createdUser = _context.Users.Add(user);

        await _context.SaveChangeAsync();
        return createdUser.Entity();

    }

    public Task<User?> editUser(string UserID, CreateUserRequest request)
    {

    }

    public Task<User?> deleteUser(string UserID)
    {

    }

    
}