using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System.Text;

namespace TweetsService.ServiceCommunications
{
    public class EventsEmitter
    {
        private ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq" };

        private string ExchangeName = "TweetsExchange";

        private IConnection connection;
        private RabbitMQ.Client.IModel channel;

        public EventsEmitter()
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Fanout,
                durable: false,
                autoDelete: false,
                arguments: null
                );
        }

        public void AnnounceTweetHasBeenPosted(int userId, int tweetId)
        {
            var message = $"User: {userId} has posted a Tweet: {tweetId}";

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(ExchangeName, "postedTweet", null, body);
        }

        public void AnnounceCommentHasBeenPosted(int userId, int tweetId, int commentId)
        {
            var message = $"User: {userId} has posted a Comment: {commentId} on Tweet: {tweetId}";

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(ExchangeName, "postedComment", null, body);
        }

        public void AnnounceUserReaction(int userId, int tweetId, int reactionsCount)
        {
            var message = $"User: {userId} has reacted, making the reactions: {reactionsCount} on Tweet: {tweetId}";

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(ExchangeName, "postedReaction", null, body);
        }
    }
}
