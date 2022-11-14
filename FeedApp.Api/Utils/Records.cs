using FeedApp.Common.Enums;

namespace FeedApp.Api.Utils;

public class Records
{
    public record PollStats(int OptionOneCount, int OptionTwoCount, UserAnswer? UserAnswer);
}