using FeedApp.Api.Models.Web;
using FeedApp.Api.Proxies.Data;
using FeedApp.Common.Models.Entities;
using FeedApp.Common.Exceptions;

namespace FeedApp.Api.Services;


public class VoteService : IVoteService
{
    private readonly DataContext _context;
    private readonly IPollService _pollService;

    public VoteService(DataContext context, IPollService pollService)
    {
        _context = context;
        _pollService = pollService;
    }

    public async Task<Vote> createVote(User? user, VoteCreateRequest request)
    {
        var pincode = request.PollPincode;
        var poll = await _pollService.GetPollByPincode(pincode);

        if (poll == null) throw new NotFoundException($"Poll with pincode {pincode} doesn't exist.");

        var vote = new Vote();
        vote.OptionSelected = request.OptionSelected;
        if (user != null)
        {
            //A user can only vote once on each poll
            var votes = _context.Votes.Where(v => v.UserId == user.Id);
            foreach (var v in votes)
            {
                if (v.PollId == poll.Id)
                {
                    throw new EfCoreException("You have already voted on this poll.");
                }
            }
            vote.UserId = user.Id;
        }
        
        vote.User = user;
        vote.PollId = poll.Id;
        vote.Poll = poll;

        var createdVote = _context.Votes.Add(vote);

        await _context.SaveChangesAsync();

        return createdVote.Entity;
    }
}
