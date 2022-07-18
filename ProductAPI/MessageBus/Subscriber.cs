using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ProductAPI.MessageBus
{
    public class Subscriber : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<Subscriber> logger;
        private IConnection connection;
        private IModel channel;

        public Subscriber(IConfiguration configuration, ILogger<Subscriber> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
            InitilizeConnection();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(queue: RabbitConstValues.QueueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var notificationMessage = Encoding.UTF8.GetString(e.Body.ToArray());
            logger.LogInformation(notificationMessage);
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
                channel.ExchangeDeclare(exchange: RabbitConstValues.ExchangeName, type: ExchangeType.Fanout);
                channel.QueueDeclare(queue: RabbitConstValues.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: RabbitConstValues.QueueName, exchange: RabbitConstValues.ExchangeName, "", null);
                logger.LogInformation("Connected to RabbitMQ Messagebus");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }

    }
}
