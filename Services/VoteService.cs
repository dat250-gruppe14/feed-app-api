using FeedAppApi.Models.Web;
using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Enums;

namespace FeedAppApi.Services;


public class VoteService : IVoteService
{
    private readonly DataContext _context;

    public VoteService(DataContext context)
    {
        _context = context;
    }

    public async Task<Vote?> createVote(Guid userId, VoteCreateRequest request)
    {
        var user = await _context.Users.FindAsync(userId);
        var pollId = request.PollId;

        var poll = await _context.Polls.FindAsync(pollId);

        if (poll == null | user == null) return null;

        var vote = new Vote();
        vote.Id = Guid.NewGuid();
        vote.OptionSelected = request.OptionSelected;
        vote.UserId = userId;
        vote.User = user;
        vote.PollId = pollId;
        vote.Poll = poll;

        var createdVote = _context.Votes.Add(vote);

        await _context.SaveChangesAsync();

        return createdVote.Entity;
    }
}