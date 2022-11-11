using FeedApp.Api.Proxies.Data;
using FeedApp.Common.Models.Entities;
using FeedApp.Messaging.Sender;

namespace FeedApp.Service
{
    public class PublishService
    {
        private readonly ILogger<PublishService> _logger;
        private readonly DataContext _context;
        private readonly IPollExpiredSender _sender;

        public PublishService(
            ILogger<PublishService> logger,
            DataContext context,
            IPollExpiredSender sender)
        {
            _logger = logger;
            _context = context;
            _sender = sender;
        }

        public async Task DoPublishAsync()
        {
            await Task.Run(() =>
            {
                var expiredPolls = _context.Polls.Where(PollHasEnded).ToList();
                foreach (var expiredPoll in expiredPolls)
                {
                    // TODO: Add mapping from Poll to PollPub
                    // var mappedPoll = _mapper.MapPollToPub(expiredPoll);
                    // _sender.SendPoll(mappedPoll);
                    // _logger.LogInformation($"Sent poll with id {mappedPoll.id}");
                }
            });
            _logger.LogInformation("PublishService did something");
        }

        private bool PollHasEnded(Poll poll) => poll.EndTime > DateTime.Now;
    }
}
