
using Confluent.Kafka;

namespace InventoryConsumer.Services
{
    public class ConsumerService : BackgroundService
    {
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<ConsumerService> _logger;
        private readonly IConfiguration _configuration;

        public ConsumerService(IConfiguration configuration, ILogger<ConsumerService> logger)
        {
            _logger = logger;
            _configuration = configuration;
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = "inventory-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("inventory-updates");

            while (!stoppingToken.IsCancellationRequested)
            {
                ProcessKafkaMessage(stoppingToken);

                Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _consumer.Close();
        }

        private void ProcessKafkaMessage(CancellationToken stoppingToken)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);

                var message = consumeResult.Message.Value;

                _logger.LogInformation($"Received inventory update: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing Kafka message: {ex.Message}");
            }
        }
    }
}
