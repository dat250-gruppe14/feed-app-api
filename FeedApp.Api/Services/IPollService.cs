using FeedApp.Common.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace FeedApp.Api.Services;

public interface IPollService {
	public Task<IEnumerable<Poll>> GetPolls(User? user);
	public Task<Poll?> GetPollByPincode(string pincode);
	public Task<Poll> CreatePoll(Poll poll);
	public Task<Poll?> DeletePoll(string pincode, Guid? userId);
	public Task<Poll?> PatchPoll(string pincode, JsonPatchDocument<Poll> pollDocument);
}
