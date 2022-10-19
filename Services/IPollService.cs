
using FeedAppApi.Models;

namespace FeedAppApi.Services;

public interface IPollService {
	public Poll getPollById(string id);
}
