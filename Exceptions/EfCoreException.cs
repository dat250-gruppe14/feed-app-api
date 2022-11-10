namespace FeedAppApi.Exceptions;

public class EfCoreException : Exception
{
    public EfCoreException(string message) : base(message) {}
}