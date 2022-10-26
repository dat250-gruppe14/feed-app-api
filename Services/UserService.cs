using FeddBackApi.Models;

namespace FeedAppApi.Services;



public class UserService : IUserService
{
    private readonly DataContext _context;

    public UserService(DataContext context)
    {
        _context = context;
    }

    public User addUser(CreateUserRequest req)
    {
        _context.DataBase.EnsureCreated();

        var user = new User();
        user.id = Guid.NewGuid().ToString();

        user.Name = req.Name;

        user.isAdmin = false;

        _context.
    }

    public User? editUer(string UserID, CreateUserRequest request)
    {

    }

    public User? deleteUser(string UserID)
    {

    }

    
}