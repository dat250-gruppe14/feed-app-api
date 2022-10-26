using FeedAppApi.Enums;

namespace FeedAppApi.Utils;

public class Records
{
    public record PollStats(int OptionOneCount, int OptionTwoCount, UserAnswer? UserAnswer);
}