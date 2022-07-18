using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace OrderAPI.MessageBus
{

    public class Subscriber : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IConfiguration configuration;
        private readonly ILogger<Subscriber> logger;
        private IConnection connection;
        private IModel channel;

        public Subscriber(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<Subscriber> logger)
        {
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
            this.logger = logger;
            InitilizeConnection();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;
            channel.BasicConsume(queue: RabbitConst.QueueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }

        private async void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var notificationMessage = Encoding.UTF8.GetString(e.Body.ToArray());
            logger.LogInformation(notificationMessage);
            IEventMessageProcessor eventProcessor = serviceProvider.GetRequiredService<IEventMessageProcessor>();
            await eventProcessor.ProcessNotification(notificationMessage);
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
                channel.QueueDeclare(queue: RabbitConst.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: RabbitConst.QueueName, exchange: RabbitConst.ExchangeName, "", null);
                logger.LogInformation("Connected to RabbitMQ Messagebus");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, ex);
            }
        }

    }
}
