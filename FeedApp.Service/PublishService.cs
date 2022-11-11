using FeedApp.Api.Proxies.Data;
using FeedApp.Common.Models.Entities;
using FeedApp.Messaging.Sender;
using FeedApp.Api.Mappers;

namespace FeedApp.Service
{
    public class PublishService
    {
        private readonly ILogger<PublishService> _logger;
        private readonly DataContext _context;
        private readonly IPollExpiredSender _sender;
        private readonly IWebMapper _mapper;

        public PublishService(
            ILogger<PublishService> logger,
            DataContext context,
            IPollExpiredSender sender,
            IWebMapper mapper)
        {
            _logger = logger;
            _context = context;
            _sender = sender;
            _mapper = mapper;
        }

        public async Task DoPublishAsync()
        {
            await Task.Run(() =>
            {
                var expiredPolls = _context.Polls
                    .Where(PollHasEnded)
                    .Where(PollEndedInInterval)
                    .ToList();

                Console.WriteLine($"expiredPolls.length = {expiredPolls.Count}");

                foreach (var expiredPoll in expiredPolls)
                {
                    var mappedPoll = _mapper.MapPollToPublish(expiredPoll);
                    _sender.SendPoll(mappedPoll);
                    _logger.LogInformation($"Sent poll with id {mappedPoll.Id}");
                }
            });
        }

        private bool PollHasEnded(Poll poll) => poll.EndTime < DateTime.Now;

        private bool PollEndedInInterval(Poll poll) => poll.EndTime > DateTime.Now.AddMinutes(-1);
    }
}
