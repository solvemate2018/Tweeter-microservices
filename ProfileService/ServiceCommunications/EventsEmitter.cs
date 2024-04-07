using RabbitMQ.Client;
using System.Text;

namespace ProfileService.ServiceCommunications
{
    public class EventsEmitter
    {
        private ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq" };

        private string exhangeName = "ProfileExchange";

        private IConnection connection;
        private IModel channel;

        public EventsEmitter()
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: exhangeName,
                type: ExchangeType.Fanout,
                durable: false,
                autoDelete: false,
                arguments: null
                );
        }

        public void AnnounceUserWasUnfollowed(int userId, int unfollowedUserId)
        {
            var message = $"User: {userId}, Unfollowed: {unfollowedUserId}";

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exhangeName, "unfollowedUser", null, body);
        }

        public void AnnounceUserWasFollowed(int userId, int followedUserId)
        {
            var message = $"User: {userId}, Followed: {followedUserId}";

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exhangeName, "followedUser", null, body);
        }
    }
}
