using FeedAppApi.Models.Web;
using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;
using FeedAppApi.Enums;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FeedAppApi.Services;


public class VoteService : IVoteService
{
    private readonly DataContext _context;
    private readonly IPollService _pollService;

    public VoteService(DataContext context, IPollService pollService)
    {
        _context = context;
        _pollService = pollService;
    }

    public async Task<Vote?> createVote(User user, VoteCreateRequest request)
    {
        var pincode = request.PollPincode;

        var poll = await _pollService.GetPollByPincode(pincode);

        if (poll == null | user == null) return null;

        var vote = new Vote();
        vote.Id = Guid.NewGuid();
        vote.OptionSelected = request.OptionSelected;
        vote.UserId = user.Id;
        vote.User = user;
        vote.PollId = poll.Id;
        vote.Poll = poll;

        var createdVote = _context.Votes.Add(vote);

        await _context.SaveChangesAsync();

        return createdVote.Entity;
    }







    
}