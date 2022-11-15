using System.Linq.Expressions;
using FeedApp.Common.Enums;
using FeedApp.Common.Models.Entities;
using FeedApp.Api.Proxies.Data;
using Microsoft.EntityFrameworkCore;

namespace FeedApp.Api.Utils;

public class PollUtils : IPollUtils
{
    public Records.PollStats CountPollVotes(Poll poll, Guid? userId)
    {
        var optionOneCount = 0;
        var optionTwoCount = 0;
        UserAnswer? userAnswer = null;

        poll.Votes ??= new List<Vote>();
        poll.DeviceVotes ??= new List<DeviceVote>();

        foreach (var vote in poll.Votes.AsParallel())
        {
            if (vote.OptionSelected == UserAnswer.AnwerOne)
                optionOneCount = Interlocked.Increment(ref optionOneCount);
            if (vote.OptionSelected == UserAnswer.AnswerTwo)
                optionTwoCount = Interlocked.Increment(ref optionTwoCount);
            if (vote.User != null && userId != null && vote.User.Id.Equals(userId))
            {
                userAnswer = vote.OptionSelected;
            }
        }
        foreach (var vote in poll.DeviceVotes.AsParallel())
        {
			optionOneCount = Interlocked.Add(ref optionOneCount, vote.Option1);
			optionTwoCount = Interlocked.Add(ref optionTwoCount, vote.Option2);
        }

        return new Records.PollStats(optionOneCount, optionTwoCount, userAnswer);
    }

    public async Task<IEnumerable<Poll>> GetOngoingPollsAuth(DataContext context, User? user)
    {
        // Not logged in user (anonymous)
        if (user == null)
        {
            return context.Polls
                .Where(PollIsPublic).AsEnumerable()
                .Where(PollIsActive)
                .ToList();
        }

        // Admin
        if (user.Role == UserRole.Admin)
        {
            return await context.Polls.ToListAsync();
        }

        // Logged in user
        // TODO: Can we filter on db-call instead after fetching?
        return context.Polls
            .ToList()
            .Where(poll => (PollIsPublic(poll) && PollIsActive(poll)) || YouOwnThePoll(poll, user.Id));
    }

    public string GeneratePincode()
    {
        Random generator = new Random();
        return generator.Next(100000, 999999).ToString("D6");
    }

    private Func<Poll, bool> PollIsPublic = poll => poll.Access == PollAccess.Public;

    private Func<Poll, Guid, bool> YouOwnThePoll = (poll, userId) => poll.OwnerId == userId;

    public bool PollIsActive(Poll poll)
    {
        var pollHasStarted = poll.StartTime < DateTime.Now;
        if (!pollHasStarted) return false;

        var pollIsAlwaysOpen = poll.EndTime == null;
        if (pollIsAlwaysOpen) return true;

        var pollHasEnded = poll.EndTime < DateTime.Now;
        return !pollHasEnded;
    }
}
