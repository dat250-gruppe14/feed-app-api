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
    private readonly IDweetMessagingService _dweetService;

    public VoteService(
        DataContext context, 
        IPollService pollService,
        IDweetMessagingService dweetService)
    {
        _context = context;
        _pollService = pollService;
        _dweetService = dweetService;
    }

    public async Task<Vote?> createVote(User user, VoteCreateRequest request)
    {

        var pincode = request.PollPincode;

        var poll = await _pollService.GetPollByPincode(pincode);

        if (poll == null) return null;

        var vote = new Vote();
        vote.Id = Guid.NewGuid();
        vote.OptionSelected = request.OptionSelected;
        if (user != null)
        {
            //A user can only vote once on each poll
            var votes = _context.Votes.Where(v => v.UserId == user.Id);
            foreach (var v in votes)
            {
                if (v.PollId == poll.Id)
                {
                    return null;
                }
            }
            vote.UserId = user.Id;
        }
        
        vote.User = user;
        vote.PollId = poll.Id;
        vote.Poll = poll;

        var createdVote = _context.Votes.Add(vote);

        await _context.SaveChangesAsync();

        _dweetService.Post(vote.Poll);

        return createdVote.Entity;
    }

    
}