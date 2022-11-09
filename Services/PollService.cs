using FeedAppApi.Exceptions;
using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;

namespace FeedAppApi.Services;

public class PollService : IPollService
{
    private readonly DataContext _context;
    private readonly IUserService _userService;
    private readonly IPollUtils _pollUtils;

    public PollService(DataContext context, IUserService userService, IPollUtils pollUtils)
    {
        _context = context;
        _userService = userService;
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

    public async Task<Poll> CreatePoll(Poll poll, User? user)
    {
        string pincode;
        Poll? pollWithSamePincode;

        do
        {
            pincode = _pollUtils.GeneratePincode();
            pollWithSamePincode = await GetPollByPincode(pincode);
        } while (pollWithSamePincode != null);

        poll.Pincode = pincode;
        poll.CreatedTime = DateTime.Now.ToUniversalTime();
        poll.OwnerId = user!.Id;

        try
        {
            var createdPoll = _context.Polls.Add(poll);
            await _context.SaveChangesAsync();
            return createdPoll.Entity;
        }
        catch (DbUpdateException e)
        {
            throw new EfCoreException(e.InnerException?.Message ?? "An error occured while storing the poll.");
        }
    }

    public async Task<Poll?> DeletePoll(string pincode, Guid? userId)
    {
        var poll = await GetPollByPincode(pincode);

        if (poll == null)
            return null;
        if (userId == null || poll.Owner.Id != userId)
            throw new NoAccessException($"User doesn't own this poll");
        
        _context.Polls.Remove(poll);
        await _context.SaveChangesAsync();

        return poll;
    }
    
    public async Task<Poll?> PatchPoll(string pincode, JsonPatchDocument<Poll> pollDocument)
    {
        var poll = await GetPollByPincode(pincode);

        if (poll == null)
        {
            return poll;
        }
        pollDocument.ApplyTo(poll);
        await _context.SaveChangesAsync();

        return poll;
    }
}

