using FeedAppApi.Models.Web;
using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Enums;

namespace FeedAppApi.Services;



public class UserService : IUserService
{
    private readonly DataContext _context;

    public UserService(DataContext context)
    {
        _context = context;
    }

    public async Task<User> createUser(User user)
    {

        //Vet ikke vi kan generere id-en slik
        user.Id = Guid.NewGuid();
        user.Role = UserRole.User;
        var createdUser = _context.Users.Add(user);

        await _context.SaveChangesAsync();
        return createdUser.Entity;

    }

    public async Task<User?> editUser(Guid Id, User newUser)
    {
        var requestUser = await _context.Users.FindAsync(Id); //User that sent the request
        var toUpdate = await _context.Users.FindAsync(newUser.Id); //User to be updated
        if (toUpdate == null | requestUser == null) return null;

        if (!toUpdate.Equals(requestUser) & !(requestUser.Role).Equals(UserRole.Admin)) return null;

        toUpdate.Email = newUser.Email;
        toUpdate.Name = newUser.Name;

        if (requestUser.Role == UserRole.Admin) //Admins can make other users admins
        {
            toUpdate.Role = newUser.Role;
        }

        _context.SaveChanges();

        return toUpdate;

    }

    public Task<User?> deleteUser(string UserID)
    {
        return null;
    }

    
}