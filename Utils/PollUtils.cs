using System.Linq.Expressions;
using FeedAppApi.Enums;
using FeedAppApi.Models.Entities;
using FeedAppApi.Proxies.Data;
using Microsoft.EntityFrameworkCore;

namespace FeedAppApi.Utils;

public class PollUtils : IPollUtils
{
    public Records.PollStats CountPollVotes(Poll poll, Guid? userId)
    {
        var optionOneCount = 0;
        var optionTwoCount = 0;
        UserAnswer? userAnswer = null;

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

    private Func<Poll, bool> PollIsPublic = poll => poll.Access == PollAccess.Public;

    private Func<Poll, Guid, bool> YouOwnThePoll = (poll, userId) => poll.OwnerId == userId;

    private bool PollIsActive(Poll poll)
    {
        var pollHasStarted = poll.StartTime < DateTime.Now;
        if (!pollHasStarted) return true;

        var pollIsAlwaysOpen = poll.EndTime == null;
        if (pollIsAlwaysOpen) return true;

        var pollHasEnded = poll.EndTime > DateTime.Now;
        return !pollHasEnded;
    }
}