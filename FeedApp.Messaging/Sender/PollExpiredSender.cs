using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Newtonsoft.Json;

using FeedApp.Messaging.Options;
using FeedApp.Common.Models.Entities;

namespace FeedApp.Messaging.Sender
{
    public class PollExpiredSender : IPollExpiredSender
    {
        private readonly string _hostname;
        private readonly string _queuename;
        private readonly string _username;
        private readonly string _password;
        private IConnection _connection;

        public PollExpiredSender(IOptions<RabbitMqConfiguration> options)
        {
            _hostname = options.Value.Hostname;
            _queuename = options.Value.QueueName;
            _username = options.Value.UserName;
            _password = options.Value.Password;

            CreateConnection();
        }

        public void SendPoll(Poll poll)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queuename, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var json = JsonConvert.SerializeObject(poll, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    });
                    var body = Encoding.UTF8.GetBytes(json);

                    Console.WriteLine($"Publishing {body}");
                    channel.BasicPublish(exchange: string.Empty, routingKey: _queuename, basicProperties: null, body: body);
                }
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };

                _connection = factory.CreateConnection();
            } catch (Exception ex)
            {
                // TODO: Switch to logger
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}
