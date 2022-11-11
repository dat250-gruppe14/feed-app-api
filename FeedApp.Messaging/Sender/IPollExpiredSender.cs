using FeedApp.Common.Models.Entities;

namespace FeedApp.Messaging.Sender
{
    public interface IPollExpiredSender
    {
        void SendPoll(Poll poll);
    }
}
