using FeedApp.Common.Models.Entities;
using FeedApp.Api.Proxies.Data;
using FeedApp.Common.Enums;
using FeedApp.Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace FeedApp.Api.Services;

public class UserService : IUserService
{
    private readonly DataContext _context;
    private readonly IAuthUtils _authUtils;
    private readonly IPasswordUtils _passwordUtils;

    public UserService(DataContext context, IAuthUtils authUtils, IPasswordUtils passwordUtils)
    {
        _context = context;
        _authUtils = authUtils;
        _passwordUtils = passwordUtils;
    }

    public async Task<User> createUser(User user)
    {
        var maill = user.Email;
        var name = user.Name;


        if (!_context.Users.Any(x => x.Email == user.Email &
                                     x.Name == user.Name))
        {
            user.Id = Guid.NewGuid();
            user.Role = UserRole.User;

            var createdUser = _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return createdUser.Entity;
        }

        return null;
    }

    public async Task<User?> editUser(User currentUser, Guid Id, User newUser)
    {
        //var currentUser = await _context.Users.FindAsync(Id); //User that sent the request
        var userToUpdate = await _context.Users.FindAsync(Id); //User to be updated
        if (userToUpdate == null | currentUser == null) return null;

        if ((currentUser.Id != userToUpdate.Id) & !(currentUser.Role).Equals(UserRole.Admin)) return null;

        userToUpdate.Email = newUser.Email;
        userToUpdate.Name = newUser.Name;

        if (currentUser.Role == UserRole.Admin) //Admins can make other users admins
        {
            userToUpdate.Role = newUser.Role;
        }

        _context.SaveChanges();

        return userToUpdate;
    }

    public Task<User?> deleteUser(string UserID)
    {
        return null;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> UpdateRefreshToken(Guid userId, string refreshToken)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return null;

        var hashedRefreshToken = _passwordUtils.HashPassword(refreshToken, user.PasswordSalt);
        
        user.RefreshToken = hashedRefreshToken;
        user.RefreshTokenExpires = DateTime.Now.AddDays(7).ToUniversalTime();

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public User? GetLoggedInUser(HttpContext httpContext)
    {
        var token = _authUtils.GetTokenFromHttpContext(httpContext);
        if (token == null) return null;
        if (!_authUtils.ValidateToken(token)) return null;
        
        var userId = _authUtils.GetUserIdFromToken(token);
        if (userId == null) return null;
        
        return _context.Users.Where(u => u.Id == Guid.Parse(userId))
            .Include(u => u.Polls)
            .Include(u => u.Votes)
            .FirstOrDefault();
    }

    public async Task<IEnumerable<User>?> GetAllUsers(User? user)
    {
        if (user is not {Role: UserRole.Admin}) return null;
        return await _context.Users.ToListAsync();
    }
}
