using RabbitMQ.Client;
using System.Text;

namespace GatewayService.ServiceCommunications
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

        public void AnnounceUserRegistered(int id, string firstName, string lastName)
        {
            var message = $"User: {id + " " + firstName + " " + lastName}";

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exhangeName, "", null, body);
        }
    }
}
