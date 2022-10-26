using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;
using Microsoft.EntityFrameworkCore;

namespace FeedAppApi.Services;

public class PollService : IPollService
{
    private readonly DataContext _context;

    public PollService(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Poll>> GetPolls()
    {
        var polls = await _context.Polls.ToListAsync();
        // TODO: Filtrer etter access
        return polls;
    }

    public async Task<Poll?> GetPollByPincode(string pincode)
    {
        var poll = await _context.Polls.FirstOrDefaultAsync(p => p.Pincode == pincode);
        return poll;
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
}

