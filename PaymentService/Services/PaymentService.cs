using AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using PaymentService.Models;
using PaymentService.Repositories;
using RabbitMQ.Client;
using Shared.Dtos.Payment;
using Shared.Enums;
using Shared.Exceptions;
using System.Text.Json;

namespace PaymentService.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly IMapper _mapper;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConnection _rabbitMqConnection;

    public PaymentService(
        IPaymentRepository repository,
        IMapper mapper,
        IHttpClientFactory httpClientFactory,
        IConnection rabbitMqConnection)
    {
        _repository = repository;
        _mapper = mapper;
        _httpClientFactory = httpClientFactory;
        _rabbitMqConnection = rabbitMqConnection;
    }
    public async Task<OrderStatus> ProcessPaymentAsync(PaymentDto payment, string apiKey, string apiSecret)
    {
        await CheckOrderAsync(payment.OrderId, apiKey, apiSecret);

        await _repository.CreatePaymentAsync(_mapper.Map<Payment>(payment));

        SendToService(payment);

        return OrderStatus.Processing;
    }

    private async Task CheckOrderAsync(int orderId, string apiKey, string apiSecret)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7001/orders/{orderId}");

        request.Headers.Add("ApiKey", apiKey);
        request.Headers.Add("ApiSecret", apiSecret);

        var response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
            throw new NotFoundException("Order not found");
    }

    public void SendToService(PaymentDto paymentDto)
    {
        var lastDigit = int.Parse(paymentDto.CardNumber[^1..]);
        var queueName = lastDigit % 2 == 0 ? "serviceAQueue" : "serviceBQueue";

        using var channel = _rabbitMqConnection.CreateModel();
        channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var message = JsonSerializer.Serialize(paymentDto);
        var body = System.Text.Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
    }
}
