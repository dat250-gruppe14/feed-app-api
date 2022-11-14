namespace FeedApp.Common.Exceptions;

public class NoAccessException : Exception
{
    public NoAccessException(string message) : base(message)
    {
        
    }
}