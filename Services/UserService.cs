using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Enums;
using Microsoft.EntityFrameworkCore;

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
        user.Role = UserRole.User;
        var createdUser = _context.Users.Add(user);

        await _context.SaveChangesAsync();
        return createdUser.Entity;
    }

    public async Task<User?> editUser(Guid Id, User newUser)
    {
        var toUpdate = await _context.Users.FindAsync(Id);
        if (toUpdate == null) return null;

        toUpdate.Email = newUser.Email;
        toUpdate.Name = newUser.Name;
        //TODO: Oppdatere role p� en trygg m�te

        toUpdate.Polls = newUser.Polls;
        toUpdate.Votes = newUser.Votes;

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
}