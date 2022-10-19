
using FeedAppApi.Models;

namespace FeedAppApi.Services;

public class PollService : IPollService
{
    public Poll getPollById(string id)
    {
	    var poll = new Poll();
	    poll.Id = id;
	    poll.Name = "Test";

	    return poll;
    }
}

