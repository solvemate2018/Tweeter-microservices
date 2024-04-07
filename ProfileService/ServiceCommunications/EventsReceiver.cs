using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProfileService.Models;
using ProfileService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace TweetsService.ServiceCommunications
{
    public class EventsReceiver
    {
        private ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq" };

        private IConnection connection;
        private IModel channel;
        private readonly IServiceProvider _serviceProvider;

        public EventsReceiver(IServiceProvider serviceProvider)
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            this._serviceProvider = serviceProvider;
            SetUserLoggedReceiver();
        }

        private void SetUserLoggedReceiver()
        {
            channel.ExchangeDeclare("ProfileExchange", ExchangeType.Fanout);

            channel.QueueDeclare("UserRegistered", false, false, false, null);

            channel.QueueBind("UserRegistered", "ProfileExchange", "");

            var consumer = new EventingBasicConsumer(channel);
            ProfileService.Services.ProfileService profileService = (ProfileService.Services.ProfileService)_serviceProvider.GetRequiredService(typeof(ProfileService.Services.ProfileService));
            consumer.Received += (ch, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var messageArr = message.Split(": ");
                UserProfile profile = new UserProfile();

                profile.Id = Int32.Parse(messageArr[1].Split(" ")[0]);
                profile.FirstName = messageArr[1].Split(" ")[1];
                profile.LastName = messageArr[1].Split(" ")[2];

                profileService.CreateProfileIfNotExists(profile);

                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume("UserRegistered", false, consumer);
        }
    }
}
