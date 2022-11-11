using FeedApp.Common.Models.Messaging;

namespace FeedApp.Messaging.Sender
{
    public interface IPollExpiredSender
    {
        void SendPoll(PollPub poll);
    }
}
