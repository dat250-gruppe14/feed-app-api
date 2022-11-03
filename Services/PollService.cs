using FeedAppApi.Exceptions;
using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace FeedAppApi.Services;

public class PollService : IPollService
{
    private readonly DataContext _context;
    private readonly IPollUtils _pollUtils;

    public PollService(DataContext context, IPollUtils pollUtils)
    {
        _context = context;
        _pollUtils = pollUtils;
    }

    public async Task<IEnumerable<Poll>> GetPolls(User? user)
    {
        return await _pollUtils.GetOngoingPollsAuth(_context, user);
    }

    public async Task<Poll?> GetPollByPincode(string pincode)
    {
        return await _context.Polls.FirstOrDefaultAsync(p => p.Pincode == pincode);
    }

    public async Task<Poll> CreatePoll(Poll poll)
    {
        // TODO: Autogenerer pin
        poll.Pincode = "919191";
        poll.CreatedTime = DateTime.Now.ToUniversalTime();
        
        // TODO: Catch DbUpdateException
        var createdPoll = _context.Polls.Add(poll);
        await _context.SaveChangesAsync();
        return createdPoll.Entity;
    }

    public async Task<Poll?> DeletePoll(string pincode, User user)
    {
        var poll = await GetPollByPincode(pincode);
        
        if (poll == null)
            throw new NotFoundException($"Poll with pincode {pincode} doesn't exist.");
        if (poll.Owner.Id != user.Id)
            throw new NoAccessException($"User doesn't own this poll");
        
        _context.Polls.Remove(poll);
        await _context.SaveChangesAsync();
        return poll;
    }
}

