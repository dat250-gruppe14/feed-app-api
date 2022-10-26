using FeedAppApi.Models.Entities;

namespace FeedAppApi.Services;

public interface IPollService {
	public Task<IEnumerable<Poll>> GetPolls();
	public Task<Poll?> GetPollByPincode(string pincode);
	public Task<Poll> CreatePoll(Poll poll);
}
