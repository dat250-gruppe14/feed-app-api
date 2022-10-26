
using FeedAppApi.Models;
using FeedAppApi.Proxies.Data;

namespace FeedAppApi.Services;

public class PollService : IPollService
{
    private readonly DataContext _context;

    public PollService(DataContext context)
    {
        _context = context;
    }

    public IEnumerable<Poll>? getPolls()
    {
        var poll = _context.Polls?.ToList();

        return poll;
    }

    public Poll? getPollById(string id)
    {
        var poll = _context.Polls?.FirstOrDefault(poll => poll.Id == id);

        return poll;
    }

    public Poll createPoll(CreatePollRequest request)
    {
        _context.Database.EnsureCreated();
 	var poll = new Poll();
 	var fooDto = mapper.Map<Poll>(request);
        poll.Id = Guid.NewGuid().ToString();

        poll.Name = request.Name;
        poll.Question = request.Question;

        _context.Polls?.Add(poll);
        _context.SaveChanges();

        return poll;
    }
}

