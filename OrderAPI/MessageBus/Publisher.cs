using RabbitMQ.Client;
using System.Text;

namespace OrderAPI.MessageBus
{
    public class Publisher : IPublisher
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<Publisher> logger;
        private IConnection connection;
        private IModel channel;

        public Publisher(IConfiguration configuration, ILogger<Publisher> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            InitilizeConnection();
        }

        private void InitilizeConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQHost"],
                Port = int.Parse(configuration["RabbitMQPort"])
            };

            try
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.ExchangeDeclare(exchange: RabbitConst.ExchangeName, type: ExchangeType.Fanout);
                logger.LogInformation("Connected to RabbitMQ Messagebus");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: RabbitConst.ExchangeName, routingKey: "", basicProperties: null, body: body);
            logger.LogInformation("Published Message  : {message}", message);
        }

        public void Dispose()
        {
            if (channel.IsOpen)
            {
                channel.Close();
                connection.Close();
            }
        }

    }
}
