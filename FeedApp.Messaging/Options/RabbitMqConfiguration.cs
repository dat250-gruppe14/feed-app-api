using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeedApp.Messaging.Options
{
    public class RabbitMqConfiguration
    {
        public string Hostname { get; set; } = "localhost";
        public string QueueName { get; set; } = "polls";
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
