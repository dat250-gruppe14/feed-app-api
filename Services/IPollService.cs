
using FeedAppApi.Models;

namespace FeedAppApi.Services;

public interface IPollService {
	public IEnumerable<Poll>? getPolls();
	public Poll? getPollById(string id);
	public Poll createPoll(CreatePollRequest request);
}
