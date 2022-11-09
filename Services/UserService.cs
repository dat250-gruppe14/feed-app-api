using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Enums;
using FeedAppApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace FeedAppApi.Services;

public class UserService : IUserService
{
    private readonly DataContext _context;
    private readonly IAuthUtils _authUtils;

    public UserService(DataContext context, IAuthUtils authUtils)
    {
        _context = context;
        _authUtils = authUtils;
    }

    public async Task<User> createUser(User user)
    {
        var maill = user.Email;
        var name = user.Name;
        

        if(!_context.Users.Any(x => x.Email == user.Email  &
                                         x.Name == user.Name))
        {
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
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = DateTime.Now.AddDays(7).ToUniversalTime();

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public User? GetLoggedInUser(HttpContext httpContext)
    {
        var token = _authUtils.GetTokenFromHttpContext(httpContext);
        if (token == null) return null;
        var userId = _authUtils.GetUserIdFromToken(token);
        if (userId == null) return null;
        
        return _context.Users.Where(u => u.Id == Guid.Parse(userId))
            .Include(u => u.Polls)
            .Include(u => u.Votes)
            .FirstOrDefault();
    }
}