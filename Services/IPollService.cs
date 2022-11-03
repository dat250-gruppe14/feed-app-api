using FeedAppApi.Models.Entities;

namespace FeedAppApi.Services;

public interface IPollService {
	public Task<IEnumerable<Poll>> GetPolls(User? user);
	public Task<Poll?> GetPollByPincode(string pincode);
	public Task<Poll> CreatePoll(Poll poll);
	public Task<Poll?> DeletePoll(string pincode, User user);
}
