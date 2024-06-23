using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Shared.Dtos.Payment;
using Shared.Enums;

namespace Shared.RabbitMq;
public class RabbitMqConsumer : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;

    public RabbitMqConsumer(string rabbitMqConnectionString, string queueName)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(rabbitMqConnectionString)
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _queueName = queueName;

        _channel.QueueDeclare(queue: _queueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);
    }

    public void StartConsuming()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (sender, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var paymentDto = JsonSerializer.Deserialize<PaymentDto>(message);

            Console.WriteLine($"Received message: {message}");

            SendToOrderService(message);

            _channel.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(queue: _queueName,
                              autoAck: false,
                              consumer: consumer);

        Console.WriteLine($"RabbitMQ consumer for queue '{_queueName}' started.");
    }

    private void SendToOrderService(string message)
    {
        using var channel = _connection.CreateModel();

        var queueName = "orderServiceQueue";
        channel.QueueDeclare(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = GenerateMessage(message);

        channel.BasicPublish(exchange: "",
                             routingKey: queueName,
                             basicProperties: null,
                             body: body);

        Console.WriteLine($"Message sent to orderServiceQueue");
    }

    private byte[] GenerateMessage(string oldMessage)
    {
        var paymentDto = JsonSerializer.Deserialize<PaymentDto>(oldMessage);

        var random = new Random();

        var notify = new PaymentStatusDto
        {
            OrderId = paymentDto.OrderId,
            OrderStatus = random.Next(2) == 0 ? OrderStatus.Completed : OrderStatus.Reject
        };

        var message = JsonSerializer.Serialize(notify);

        return Encoding.UTF8.GetBytes(message); 
    }
    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}

