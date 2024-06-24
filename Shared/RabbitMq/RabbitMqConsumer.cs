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

    public RabbitMqConsumer(string queueName)
    {
        var factory = new ConnectionFactory
        {
            HostName = "rabbitmq",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
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
        consumer.Received += (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Received message: {message}");

                Task.Delay(1000).Wait();

                Console.WriteLine($"Received message: {message}");

                var paymentDto = JsonSerializer.Deserialize<PaymentDto>(message);

                Console.WriteLine($"Received message: {paymentDto}");

                if (paymentDto == null)
                {
                    Console.WriteLine("Failed to deserialize message.");
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                    return;
                }

                ProcessMessage(paymentDto);

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
                _channel.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        Console.WriteLine("Starting BasicConsume...");
        _channel.BasicConsume(queue: _queueName,
                              autoAck: false,
                              consumer: consumer);

        Console.WriteLine($"RabbitMQ consumer for queue '{_queueName}' started.");
    }

    private void ProcessMessage(PaymentDto paymentDto)
    {
        try
        {
            SendToOrderService(paymentDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in ProcessMessage: {ex.Message}");
            throw;
        }
    }

    private void SendToOrderService(PaymentDto paymentDto)
    {
        try
        {
            using var channel = _connection.CreateModel();

            var queueName = "orderServiceQueue";
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = GenerateMessage(paymentDto);

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($"Message sent to orderServiceQueue for Order ID: {paymentDto.OrderId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SendToOrderService: {ex.Message}");
            throw;
        }
    }

    private byte[] GenerateMessage(PaymentDto paymentDto)
    {
        try
        {
            var random = new Random();

            var notify = new PaymentStatusDto
            {
                OrderId = paymentDto.OrderId,
                OrderStatus = random.Next(2) == 0 ? OrderStatus.Completed : OrderStatus.Reject
            };

            var message = JsonSerializer.Serialize(notify);

            return Encoding.UTF8.GetBytes(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GenerateMessage: {ex.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        Console.WriteLine("Disposing RabbitMQ consumer...");
        _channel?.Dispose();
        _connection?.Dispose();
        Console.WriteLine("RabbitMQ consumer disposed.");
    }
}

