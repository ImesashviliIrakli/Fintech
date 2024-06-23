using OrderService.Repositories;
using OrderService.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Dtos.Payment;
using System.Text;
using System.Text.Json;

namespace OrderService;

public class RabbitMqConsumer : BackgroundService
{
    private readonly string _rabbitMqConnectionString;
    private readonly string _queueName;
    private readonly IServiceScopeFactory _scopeFactory;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMqConsumer(
        string rabbitMqConnectionString,
        string queueName,
        IServiceScopeFactory scopeFactory)
    {
        _rabbitMqConnectionString = rabbitMqConnectionString;
        _queueName = queueName;
        _scopeFactory = scopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(_rabbitMqConnectionString)
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: _queueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();

            var message = Encoding.UTF8.GetString(body);

            var paymentStatusDto = JsonSerializer.Deserialize<PaymentStatusDto>(message);

            using (var scope = _scopeFactory.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderService>();
                Console.WriteLine($"Received message from {_queueName}: {message}");

                await orderRepository.CompleteOrderAsync(paymentStatusDto);
            }

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _queueName,
                              autoAck: false,
                              consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}